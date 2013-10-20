using EnvDTE;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismAddin
{
    // TemplateMethod DesignPattern
    // Abstract TraverseProjectTree navigates from solution->projects->folders->projectitems
    //    which is common to all derived classes.
    // Derived classes implement the ProjectItem processing in the ProcessFolderItem method.
    public abstract class TraverseProjectTree
    {        
        protected Solution2 sln2;
        protected List<string> currentFolderList;
        protected ProjectItem currentFolder;
        protected ProjectItem file;

        public TraverseProjectTree(Solution2 sln2) {
            this.sln2 = sln2;
        }

        protected TraverseProjectTree() {
        }

        public void TraverseAllProjects() {
            string currentProjectName = string.Empty;
            string currentFilePath = string.Empty;

            foreach (Project project in sln2.Projects) {
                currentProjectName = project.Name;
                MessageService.WriteMessage(string.Empty);
                MessageService.WriteMessage("===================================================");
                MessageService.WriteMessage(string.Format("Project: {0}", project.Name));
                MessageService.WriteMessage("===================================================");
                TraverseOneProject(project);
            }
            //SaveProjectsAndSolution();
        }

        private void TraverseOneProject(Project project) {
            foreach (ProjectItem projectItem in project.ProjectItems) {
                if (projectItem.Kind == EnvDTEConstants.vsProjectItemKindPhysicalFolder) {
                    currentFolderList = new List<string>();
                    TraverseProjectFolders(projectItem);
                }
                else {
                    if (projectItem.Kind == EnvDTEConstants.vsProjectItemKindPhysicalFile && IsKindT4(projectItem)) {
                        ProcessFolderItem(projectItem);
                        MessageService.WriteMessage(string.Empty);
                    }
                }
            }
        }

        private void TraverseProjectFolders(ProjectItem projectItem) {
            if (projectItem.Kind == EnvDTEConstants.vsProjectItemKindPhysicalFolder) {
                currentFolderList.Add(projectItem.Name);
                currentFolder = projectItem;
                foreach (ProjectItem subFolder in projectItem.ProjectItems) {
                    // recurse folders
                    TraverseProjectFolders(subFolder);
                }
            }
            else {
                TraverseFolderItems(projectItem);
                return;
            }
        }

        private void TraverseFolderItems(ProjectItem projectItem) {
            if (projectItem.Kind == EnvDTEConstants.vsProjectItemKindPhysicalFile && IsKindT4(projectItem)) {
                MessageService.WriteMessage(string.Format("TemplateFile: {0}", projectItem.Name));

                //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);
                ProcessFolderItem(projectItem);
                MessageService.WriteMessage(string.Empty);

                return;
            }
            if (projectItem.ProjectItems.Count == 0) {
                return;
            }
            else {
                foreach (ProjectItem subProjectItem in projectItem.ProjectItems) {
                    // recurse folderitems
                    TraverseFolderItems(subProjectItem);
                }
            }
        }

        protected string GetLocalPath(ProjectItem fileItem) {
            string localPath = string.Empty;
            foreach (EnvDTE.Property property in fileItem.Properties) {
                if (property.Name.ToLower() == "localpath") {
                    localPath = property.Value.ToString();
                    break;
                }
            }
            return localPath;
        }


        protected string GetPathToT4Child(ProjectItem templateItem) {
            string pathToT4Child = string.Empty;

            foreach (ProjectItem childItem in templateItem.ProjectItems) {
                if (childItem.ProjectItems.Count == 0) {
                    pathToT4Child = GetLocalPath(childItem);
                    break;
                }
                else {
                    // should not get here because templateItem should have only 1 child
                    throw new Exception("Template File has more that 1 child.");
                }
            }

            return pathToT4Child;
        }
        
        protected void DeleteT4Child(ProjectItem templateItem) {
            foreach (ProjectItem childItem in templateItem.ProjectItems) {
                if (childItem.ProjectItems.Count == 0) {
                    string localPath = GetLocalPath(childItem);
                    File.Delete(localPath);
                    MessageService.WriteMessage(string.Format("ChildFile: {0} deleted", localPath));
                }
                else {
                    // should not get here because templateItem should have only 1 child
                    throw new Exception("Template File has more that 1 child.");
                }
            }
        }

        public abstract void ProcessFolderItem(ProjectItem projectItem);
        public abstract bool IsKindT4(ProjectItem projectItem);
    }

    public class RenameT4Templates : TraverseProjectTree
    {
        public RenameT4Templates(Solution2 sln2) {
            base.sln2 = sln2;
        }

        public override void ProcessFolderItem(ProjectItem projectItem) {
            string templateFileFullName = string.Empty;
            string templateFileFullNameRenamed = string.Empty;

            try {
                file = currentFolder.ProjectItems.Item(projectItem.Name);
                // Replace the PrismTemplates file with a renamed copy.
                if (file != null) {
                    // Get the complete PrismTemplates file name including full path.
                    templateFileFullName = GetLocalPath(file);
                    // Replace the word 'Entity' in the PrismTemplates file name with the Domain EntityName.
                    templateFileFullNameRenamed = Utilities.SearchFileNameAndReplace(templateFileFullName, "Entity", Connect.settingsObject.EntityName);

                    // Make the copy.
                    File.Copy(templateFileFullName, templateFileFullNameRenamed);

                    // Delete the file that the PrismTemplates file (.tt) generated from the file system.
                    DeleteT4Child(file);

                    // Remove the PrismTemplates file from the project.
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);
                    file.Remove();
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);

                    // Delete the PrismTemplates file (.tt) from the file system.
                    File.Delete(templateFileFullName);

                    // Add the Domain named template file (.tt) to the project. (automatically adds it to file system)
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);
                    currentFolder.ProjectItems.AddFromFile(templateFileFullNameRenamed);
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);

                    MessageService.WriteMessage(string.Format("Template renamed. {0}", templateFileFullNameRenamed));
                }
                else {
                    MessageService.WriteMessage(string.Format("Template: {0} not found", projectItem.Name));
                }
            }
            catch (Exception ex) {
                if (ex.Message == "Value does not fall within the expected range.") {
                    MessageService.WriteMessage(string.Format("Template: {0} not found", projectItem.Name));
                }
                else {
                    throw ex;
                }
            }
        }

        public override bool IsKindT4(ProjectItem projectItem) {
            // Check if .tt file that contains the word 'Entity' in its Name.
            return (projectItem.Name.ToLower().IndexOf("entity") != -1) && projectItem.Name.ToLower().EndsWith(".tt");
        }
    }

    public class RemoveT4TemplatesFromSolution : TraverseProjectTree
    {
        public RemoveT4TemplatesFromSolution(Solution2 sln2) {
            base.sln2 = sln2;
        }

        public override void ProcessFolderItem(ProjectItem projectItem) {
            string templateFileFullName = string.Empty;

            try {
                file = currentFolder.ProjectItems.Item(projectItem.Name);
                // Remove the PrismTemplates file and add its generated file to the project.
                if (file != null) {
                    // Get the complete PrismTemplates file name including full path.
                    templateFileFullName = GetLocalPath(file);

                    string generatedFilePath = GetPathToT4Child(file);
                    string generatedFilePathCopy = generatedFilePath + ".gc";
                    File.Copy(generatedFilePath, generatedFilePathCopy);

                    // Add the file that the PrismTemplates file (.tt) generated to the project.
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);
                    //currentFolder.ProjectItems.AddFromFile(GetPathToT4Child(file));
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);

                    // Remove the PrismTemplates file from the project.
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);
                    file.Remove();
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);

                    // Delete the PrismTemplates file (.tt) from the file system.
                    File.Delete(templateFileFullName);
                    File.Delete(generatedFilePath);

                    //File.Move(generatedFilePathCopy, generatedFilePath);

                    // Add the file that the PrismTemplates file (.tt) generated to the project.
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);
                    currentFolder.ProjectItems.AddFromFile(generatedFilePathCopy);
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);

                    //MessageService.WriteMessage(string.Format("Template deleted. {0}", templateFileFullName));
                }
                else {
                    MessageService.WriteMessage(string.Format("Template: {0} not found", projectItem.Name));
                }
            }
            catch (Exception ex) {
                if (ex.Message == "Value does not fall within the expected range.") {
                    MessageService.WriteMessage(string.Format("Template: {0} not found", projectItem.Name));
                }
                else {
                    throw ex;
                }
            }
        }

        public override bool IsKindT4(ProjectItem projectItem) {
            // Check if .tt file.
            return projectItem.Name.ToLower().EndsWith(".tt");
        }
    }

    public class RenameGeneratedCode : TraverseProjectTree
    {
        public RenameGeneratedCode(Solution2 sln2) {
            base.sln2 = sln2;
        }

        public override void ProcessFolderItem(ProjectItem projectItem) {
            string gcFileFullName = string.Empty;
            string gcFileFullNameRenamed = string.Empty;

            try {
                file = currentFolder.ProjectItems.Item(projectItem.Name);
                // Remove the PrismTemplates file and add its generated file to the project.
                if (file != null) {
                    // Get the complete PrismTemplates file name including full path.
                    gcFileFullName = GetLocalPath(file);
                    // Replace the word 'Entity' in the PrismTemplates file name with the Domain EntityName.
                    gcFileFullNameRenamed = gcFileFullName.Substring(0, gcFileFullName.Length - 3);

                    // Make the copy.
                    File.Copy(gcFileFullName, gcFileFullNameRenamed);

                    // Remove the PrismTemplates file from the project.
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);
                    file.Remove();
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);

                    // Delete the PrismTemplates file (.tt) from the file system.
                    File.Delete(gcFileFullName);

                    // Add the Domain named template file (.tt) to the project. (automatically adds it to file system)
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);
                    currentFolder.ProjectItems.AddFromFile(gcFileFullNameRenamed);
                    //System.Threading.Thread.Sleep(Connect.settingsObject.LongSleep);

                    MessageService.WriteMessage(string.Format("GCFile renamed. {0}", gcFileFullNameRenamed));
                }
                else {
                    MessageService.WriteMessage(string.Format("Template: {0} not found", projectItem.Name));
                }
            }
            catch (Exception ex) {
                if (ex.Message == "Value does not fall within the expected range.") {
                    MessageService.WriteMessage(string.Format("Template: {0} not found", projectItem.Name));
                }
                else {
                    throw ex;
                }
            }
        }

        public override bool IsKindT4(ProjectItem projectItem) {
            // Check if .gc file.
            return projectItem.Name.ToLower().EndsWith(".gc");
        }
    }

}
