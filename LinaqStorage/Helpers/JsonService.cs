using LinaqStorage.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinaqStorage.Helpers
{
    public class JsonService
    {
        public static string FileObjectsSavedList
        {
            get
            {
                return App.AppSettings.GetValueOrDefault(nameof(FileObjectsSavedList), string.Empty);
            }
            set
            {
                App.AppSettings.AddOrUpdateValue(nameof(FileObjectsSavedList), value);
            }
        }

        public static List<FileObject> GetFilesListFromJson()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(FileObjectsSavedList))
                {
                    return JsonConvert.DeserializeObject<List<FileObject>>(FileObjectsSavedList);
                }

            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
            }
            return new List<FileObject>();

        }

        public static void AddFileToJsonList(FileObject obj)
        {
            try
            {
                List<FileObject> list = GetFilesListFromJson();
                if (list == null)
                    list = new List<FileObject>();

                if (list.FirstOrDefault(x => x.FilePath == obj.FileGuid) == null)
                    list.Add(obj);

                SaveList(list);
            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
            }
        }

        public static void RemoveFileFromList(FileObject obj)
        {
            try
            {
                List<FileObject> list = GetFilesListFromJson();
                if (list != null)
                {
                    FileObject removeObj = list.FirstOrDefault(x => x.FileGuid == obj.FileGuid);
                    if (removeObj != null)
                    {
                        list.Remove(removeObj);
                        SaveList(list);
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
            }
        }
        public static void ResetList()
        {
            List<FileObject> EmptyList = new List<FileObject>();
            SaveList(EmptyList);
        }

        public static void SaveList(List<FileObject> list)
        {
            FileObjectsSavedList = JsonConvert.SerializeObject(list, Formatting.Indented);
        }

    }
}
