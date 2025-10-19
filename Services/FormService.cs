using System.Collections.Generic;
using System.Linq;

namespace MauiApp3
{
    public static class FormService
    {
        private static List<Form> forms = new List<Form>();
        private static readonly string formsFilePath = Path.Combine(FileSystem.AppDataDirectory, "forms.txt");

        static FormService()
        {
            LoadFormsFromFile();
        }

        private static void LoadFormsFromFile()
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
                            System.Diagnostics.Debug.WriteLine($"[FormService] Loaded form: {form.formId}, Item: {form.itemName}, Status: {form.Status}");
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

        private static void SaveFormsToFile()
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

        public static List<Form> GetForms()
        {
            return new List<Form>(forms);
        }

        public static void AddForm(Form form)
        {
            form.formId = forms.Count > 0 ? forms.Max(f => f.formId) + 1 : 1;
            forms.Add(form);
            SaveFormsToFile();
            System.Diagnostics.Debug.WriteLine($"[FormService] Added new form: {form.formId}, Item: {form.itemName}");
        }

        public static void UpdateForm(Form form)
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

        public static void DeleteForm(int formId)
        {
            var form = forms.FirstOrDefault(f => f.formId == formId);
            if (form != null)
            {
                forms.Remove(form);
                SaveFormsToFile();
                System.Diagnostics.Debug.WriteLine($"[FormService] Deleted form: {formId}");
            }
        }

        public static Form GetForm(int formId)
        {
            return forms.FirstOrDefault(f => f.formId == formId);
        }
    }
}