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


        //Keeping the collection data separate
        public FormViewModel()
        {
            FormsCollection = new ObservableCollection<Form>{
            new Form { formId = 1, studentId = 1, itemId = 1, itemName = "Item 1", Notes = "Test Form 1", Status = "Pending" },
            new Form { formId = 1, studentId = 1, itemId = 1, itemName = "Item 2", Notes = "Test Form 2", Status = "Pending" },
            };

            RequestedForms = new ObservableCollection<Form>();
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
    }
}