using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace MauiApp3
{
    public class FormViewModel
    {
        public ObservableCollection<Form> FormsCollection { get; set; }
        public ObservableCollection<Form> RequestedForms { get; set; }
        public ObservableCollection<Form> ApprovedForms { get; set; }
        public ObservableCollection<Form> RejectedForms { get; set; }

        public FormViewModel()
        {
            FormsCollection = new ObservableCollection<Form>();
            RequestedForms = new ObservableCollection<Form>();
            RejectedForms = new ObservableCollection<Form>();
            ApprovedForms = new ObservableCollection<Form>();

            // Load forms from file instead of test data
            LoadFormsFromService();
        }

        private void LoadFormsFromService()
        {
            var forms = FormService.GetForms();
            FormsCollection.Clear();
            foreach (var form in forms)
            {
                FormsCollection.Add(form);
            }
        }

        public void StaffFilterRequestedForms()
        {
            RequestedForms.Clear();
            var requested = FormsCollection.Where(form =>
                form.Status.Contains("Pending", StringComparison.OrdinalIgnoreCase));

            foreach (var item in requested)
            {
                RequestedForms.Add(item);
            }
        }

        public void FilterRequestedForms()
        {
            RequestedForms.Clear();
            var currentId = Preferences.Get("CurrentId", -1);
            var requested = FormsCollection.Where(form =>
                form.Status.Contains("Pending", StringComparison.OrdinalIgnoreCase) &&
                form.studentId.Equals(currentId));

            foreach (var item in requested)
            {
                RequestedForms.Add(item);
            }
        }

        public void FilterApprovedForms()
        {
            ApprovedForms.Clear();
            var currentId = Preferences.Get("CurrentId", -1);
            var approved = FormsCollection.Where(form =>
                form.Status.Contains("Approved", StringComparison.OrdinalIgnoreCase) &&
                form.studentId.Equals(currentId));

            foreach (var item in approved)
            {
                ApprovedForms.Add(item);
            }
        }

        public void FilterRejectedForms()
        {
            RejectedForms.Clear();
            var currentId = Preferences.Get("CurrentId", -1);
            var rejected = FormsCollection.Where(form =>
                form.Status.Contains("Rejected", StringComparison.OrdinalIgnoreCase) &&
                form.studentId.Equals(currentId));

            foreach (var item in rejected)
            {
                RejectedForms.Add(item);
            }
        }

        public void RefreshFromService()
        {
            LoadFormsFromService();
            FilterRequestedForms();
            FilterApprovedForms();
            FilterRejectedForms();
        }
    }
}