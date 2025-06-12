using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App012_20250612_MVVM
{
    public class PostsViewModel : INotifyPropertyChanged
    {
        public Posts posts { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        //定義一個ICommand型別的參數，他會回傳實作ICommand介面的RelayCommand類別。
        public ICommand UpdateTitleName { get { return new RelayCommand(UpdateTitleExecute, CanUpdateTitleExecute); } }


        public PostsViewModel()
        {
            posts = new Posts { postsText = "", postsTitle = "Unknown" };
        }

        public string PostsTitle
        {
            get { return posts.postsTitle; }
            set
            {
                if (posts.postsTitle != value)
                {
                    posts.postsTitle = value;
                    RaisePropertyChanged("postsTitle");
                }
            }
        }

        //產生事件的方法
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //更新Title，原本放在View那邊的邏輯，藉由繫結的方式來處理按下Button的事件。
        void UpdateTitleExecute()
        {
            PostsTitle = "SkyMVVM";
        }

        //定義是否可以更新Title
        bool CanUpdateTitleExecute()
        {
            return true;
        }
    }
}
