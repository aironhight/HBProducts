﻿using HBProducts.Models;
using HBProducts.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HBProducts.ViewModels
{
    public class EnquiryViewModel : BaseViewModel
    {
        private Product product;
        private bool isBusy;
        private INotifyView view;
        private Customer customer;

        public EnquiryViewModel(Product product, INotifyView view)
        {
            this.product = product;
            this.view = view;
            isBusy = false;
            customer = Settings.GetUserData();
        }

        public EnquiryViewModel(INotifyView view)
        {
            this.view = view;
            isBusy = false;
            customer = Settings.GetUserData();
        }

        public Product Product
        {
            get { return product; }
        }

        public bool IsBusy
        {
            set { SetProperty(ref isBusy, value); OnPropertyChanged("IsBusy"); OnPropertyChanged("FieldsEnabled"); }
            get { return isBusy; }
        }

        public bool FieldsEnabled
        {
            get { return !isBusy; }
        }

        public async void sendMail(string text)
        {
            IsBusy = true;
            var apiKey = Constants.sendgridApiKey;
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("product-enquiry@hbapp.com", "EnquiryMessenger"),
                Subject = product != null ? "Enquiry about " + product.FullName : "Email from customer",
                PlainTextContent = "Write enquiry...!",
                HtmlContent = "<p>" + text + "</p>"
            };

            msg.AddTo(new EmailAddress("253911@via.dk", "Hristo"));
            try { 
                var response = await client.SendEmailAsync(msg);
                view.notify("response", response);
            }
            catch (HttpRequestException e)
            {
                view.notify("Error", "Error occured while sending enquiry. Check internet connection.");
            }
            catch (Exception ex)
            {
                view.notify("Error", "Unexpected error:" + Environment.NewLine  + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void saveUserData(string name, string company, string email, string telno, string country)
        {
            customer = new Customer(name, company, email, telno, country);
            Settings.SaveUserData(customer);
        }

        public void clearUserData()
        {
            Settings.ClearUserData();
        }

        public Customer Customer
        {
            get { return customer; }
        }

        public INotifyView INotifyView
        {
            get { return view; }
            set { this.view = value; }
        }
    }
}
