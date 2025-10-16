using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
//using static Java.Text.Normalizer;

namespace MauiApp3
{
    public class FormViewModel
    {
        public ObservableCollection<Form> FormsCollection { get; set; }

        public ObservableCollection<Form> RequestedForms { get; set; }
        public ObservableCollection<Form> ApprovedForms { get; set; }

        public ObservableCollection<Form> RejectedForms { get; set; }


        //Keeping the collection data separate
        public FormViewModel()
        {
            FormsCollection = new ObservableCollection<Form>{
            new Form { formId = 1, studentId = 1, itemId = 1, itemName = "Item 1", Notes = "Test Form 1", Status = "Pending" },
            new Form { formId = 1, studentId = 1, itemId = 1, itemName = "Item 2", Notes = "Test Form 2", Status = "Pending" },
            new Form { formId = 1, studentId = 1, itemId = 1, itemName = "Item 2", Notes = "Test Form 2", Status = "Approved" },
            };
            RequestedForms = new ObservableCollection<Form>();
            RejectedForms = new ObservableCollection<Form>();
            ApprovedForms = new ObservableCollection<Form>();
        }

        public void FilterRequestedForms()
        {
            RequestedForms.Clear();

            IEnumerable<Form> requested;
            {
                RequestedForms.Clear();
                requested = FormsCollection.Where(form =>
                    form.Status.Contains("Pending", StringComparison.OrdinalIgnoreCase));
            }

            foreach (var item in requested)
            {
                RequestedForms.Add(item);
            }
        }

        public void FilterApprovedForms()
        {
            ApprovedForms.Clear();

            IEnumerable<Form> approved;
            {
                ApprovedForms.Clear();
                approved = FormsCollection.Where(form =>
                    form.Status.Contains("Approved", StringComparison.OrdinalIgnoreCase));
            }

            foreach (var item in approved)
            {
                ApprovedForms.Add(item);
            }
        }

        public void FilterRejectedForms()
        {
            RejectedForms.Clear();

            IEnumerable<Form> rejected;
            {
                RejectedForms.Clear();
                rejected = FormsCollection.Where(form =>
                    form.Status.Contains("Rejected", StringComparison.OrdinalIgnoreCase));
            }

            foreach (var item in rejected)
            {
                RejectedForms.Add(item);
            }
        }
    }
}