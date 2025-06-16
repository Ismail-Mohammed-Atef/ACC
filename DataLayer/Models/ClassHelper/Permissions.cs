using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.ClassHelper
{
    public static class Permissions
    {
        // 📁 Document Management Permissions
        public const string ViewFiles = "View Files";
        public const string UploadFiles = "Upload Files";
        public const string EditFiles = "Edit Files";
        public const string DeleteFiles = "Delete Files";
        public const string MoveFiles = "Move Files";
        public const string CreateFolders = "Create Folders";
        public const string RenameFolders = "Rename Folders";
        public const string DeleteFolders = "Delete Folders";
        public const string SetPermissions = "Set Permissions";

        // 📝 Review Workflow Permissions
        public const string CreateReviews = "Create Reviews";
        public const string SubmitReviews = "Submit Reviews";
        public const string ApproveRejectReviews = "Approve/Reject Reviews";
        public const string ViewReviews = "View Reviews";

        // 📊 Data & Insights
        public const string ViewReports = "View Reports";
        public const string CreateReports = "Create Reports";

        // 🧩 Forms
        public const string CreateForms = "Create Forms";
        public const string EditForms = "Edit Forms";
        public const string ViewForms = "View Forms";
        public const string DeleteForms = "Delete Forms";

        // 🛠️ Issues & RFIs
        public const string ViewIssues = "View Issues";
        public const string CreateIssues = "Create Issues";
        public const string EditIssues = "Edit Issues";
        public const string DeleteIssues = "Delete Issues";

        public const string ViewRFIs = "View RFIs";
        public const string CreateRFIs = "Create RFIs";
        public const string RespondToRFIs = "Respond to RFIs";

        // 🧪 Models & Coordination
        public const string ViewModels = "View Models";
        public const string RunClashes = "Run Clashes";
        public const string AssignClashes = "Assign Clashes";

        // 🔧 Settings & Admin
        public const string ManageTemplates = "Manage Templates";
        public const string ManagePermissions = "Manage Permissions";
    }
}
