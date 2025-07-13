// File: ViewModels/SelectRoleViewModels.cs

using System.Collections.Generic; // Required for List<T>

namespace GolestanProject.ViewModels
{
    // This ViewModel represents a single selectable role/account option
    public class UserRoleAccountDetail
    {
        public string UniqueId { get; set; } // A unique identifier for the specific role + account combo
        public string RoleName { get; set; }
        public int? AccountId { get; set; } // Nullable int for student_id or instructor_id
        public string DisplayText { get; set; } // What will be displayed on the button/option (e.g., "Student (ID: 123)")
    }

    // This ViewModel holds the list of all available role/account options for the user
    public class SelectRoleViewModel
    {
        public List<UserRoleAccountDetail> AvailableRoleAccounts { get; set; } = new List<UserRoleAccountDetail>();
        public string SelectedUniqueId { get; set; } // To capture the selected radio button's value from the form
    }
}