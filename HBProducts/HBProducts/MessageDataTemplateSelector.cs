using HBProducts.ViewModels;
using HBProducts.ViewCells;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace HBProducts
{
    class MessageDataTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingTemplate;
        DataTemplate outgoingTemplate;

        public MessageDataTemplateSelector()
        {
            incomingTemplate = new DataTemplate(typeof(IncomingMessageViewCell));
            outgoingTemplate = new DataTemplate(typeof(OutgoingMessageViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is TextChatViewModel)
            {
                if (((TextChatViewModel)item).Direction == TextChatViewModel.ChatDirection.Incoming)
                    return incomingTemplate;
                else
                    return outgoingTemplate;
            }

            throw new Exception($"Unknown chat");
        }
    }
}
