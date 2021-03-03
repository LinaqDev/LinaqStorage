using LinaqStorage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinaqStorage.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public DetailsViewModel()
        {
            DisplayObject = new FileObject();
        }

        public DetailsViewModel(object obj)
        {
            DisplayObject = new FileObject();
            if (obj !=null && obj is FileObject)
            {
                DisplayObject = obj as FileObject;
            }
        }

        private FileObject _displayObject;
        public FileObject DisplayObject
        {
            get { return _displayObject; }
            set
            {
                _displayObject = value;
                RaisePropertyChanged("DisplayObject");
            }
        }
    }
}
