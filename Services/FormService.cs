using System.Collections.Generic;
using System.Linq;

namespace MauiApp3
{
    // Implements IDataService<Form> interface for managing Form entities
    public class FormService : IDataService<Form>
    {
        private List<Form> forms = new List<Form>();
        private readonly string formsFilePath = Path.Combine(FileSystem.AppDataDirectory, "forms.txt");

        // Singleton pattern for global access
        private static FormService _instance;
        public static FormService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FormService();
                return _instance;
            }
        }

        private FormService()
        {
            LoadFormsFromFile();
        }

        private void LoadFormsFromFile()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[FormService] Loading forms from file: {formsFilePath}");

                if (File.Exists(formsFilePath))
                {
                    var lines = File.ReadAllLines(formsFilePath);
                    System.Diagnostics.Debug.WriteLine($"[FormService] Found {lines.Length} lines in forms file");

                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var parts = line.Split('|');
                        if (parts.Length >= 5)
                        {
                            var form = new Form
                            {
                                formId = int.TryParse(parts[0], out var fId) ? fId : forms.Count + 1,
                                studentId = int.TryParse(parts[1], out var sId) ? sId : 0,
                                itemId = int.TryParse(parts[2], out var iId) ? iId : 0,
                                itemName = parts[3],
                                Notes = parts[4],
                                Status = parts.Length > 5 ? parts[5] : "Pending",
                                CollectionInstructions = parts.Length > 6 ? parts[6] : ""
                            };
                            forms.Add(form);
                            System.Diagnostics.Debug.WriteLine($"[FormService] Loaded form: {form.formId}, Item: {form.itemName}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[FormService] No existing forms file found. Starting with empty list.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FormService] Error loading forms: {ex.Message}");
            }
        }

        private void SaveFormsToFile()
        {
            try
            {
                var lines = forms.Select(f =>
                    $"{f.formId}|{f.studentId}|{f.itemId}|{f.itemName}|{f.Notes}|{f.Status}|{f.CollectionInstructions ?? ""}");
                File.WriteAllLines(formsFilePath, lines);
                System.Diagnostics.Debug.WriteLine($"[FormService] Saved {forms.Count} forms to {formsFilePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FormService] Error saving forms: {ex.Message}");
            }
        }

        // IDataService<Form> interface implementation
        public List<Form> GetAll()
        {
            return new List<Form>(forms);
        }

        public Form GetById(int id)
        {
            return forms.FirstOrDefault(f => f.formId == id);
        }

        public void Add(Form form)
        {
            form.formId = forms.Count > 0 ? forms.Max(f => f.formId) + 1 : 1;
            forms.Add(form);
            SaveFormsToFile();
            System.Diagnostics.Debug.WriteLine($"[FormService] Added new form: {form.formId}, Item: {form.itemName}");
        }

        public void Update(Form form)
        {
            var existingForm = forms.FirstOrDefault(f => f.formId == form.formId);
            if (existingForm != null)
            {
                existingForm.Status = form.Status;
                existingForm.Notes = form.Notes;
                existingForm.CollectionInstructions = form.CollectionInstructions;
                SaveFormsToFile();
                System.Diagnostics.Debug.WriteLine($"[FormService] Updated form: {form.formId}, Status: {form.Status}");
            }
        }

        public void Delete(int id)
        {
            var form = forms.FirstOrDefault(f => f.formId == id);
            if (form != null)
            {
                forms.Remove(form);
                SaveFormsToFile();
                System.Diagnostics.Debug.WriteLine($"[FormService] Deleted form: {id}");
            }
        }

        // Static wrapper methods for backward compatibility
        // These allow existing code throughout the app to continue using the old method names
        // without needing to change code throughout the app as we implemented the interfaces later
        public static List<Form> GetForms() => Instance.GetAll();
        public static Form GetForm(int formId) => Instance.GetById(formId);
        public static void AddForm(Form form) => Instance.Add(form);
        public static void UpdateForm(Form form) => Instance.Update(form);
        public static void DeleteForm(int formId) => Instance.Delete(formId);
    }
}