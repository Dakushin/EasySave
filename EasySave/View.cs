using System;
using System.Collections.Generic;


namespace EasySave
{
    abstract class View
    {
        protected ViewModel viewModel;
        public View()
        {
            viewModel = new ViewModel(this);
        }

        public abstract void Afficher();
        public abstract void AfficherText(string s);

    }
}
