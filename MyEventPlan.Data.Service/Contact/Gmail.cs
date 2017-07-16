using System.Collections.Generic;
using System.Linq;
using Google.Contacts;
using Google.GData.Client;

namespace MyEventPlan.Data.Service.Contact
{
    public class Gmail
    {
        public IEnumerable<Event.Data.Objects.Entities.Contact> GetAllGmailContacts(string ApplicationName,string userName,string passWord)
        {
           //var authSubUrl = AuthSubUtil.getRequestUrl("http://www.example.com/Welcome.asp",
           //     "https://www.google.com/m8/feeds/", false, true);
           // GAuthSubRequestFactory authFactory = new GAuthSubRequestFactory("cp",
           //     "exampleCo-exampleApp-1");
            var contacts = new List<Event.Data.Objects.Entities.Contact>();
            var contact = new Event.Data.Objects.Entities.Contact();
            RequestSettings rs = new RequestSettings(ApplicationName,userName ,passWord);
            // AutoPaging results in automatic paging in order to retrieve all contacts
            rs.AutoPaging = true;
            ContactsRequest cr = new ContactsRequest(rs);

            Feed<Google.Contacts.Contact> f = cr.GetContacts();
            foreach (Google.Contacts.Contact e in f.Entries)
            {
                contact.Firstname = e.Name.ToString();
                contact.Email = e.PrimaryEmail.Address;
                contact.Title = e.Title;
                contact.Mobile = e.Phonenumbers.FirstOrDefault().Value;
                contacts.Add(contact);
            }
            return contacts;
        }
    }
}
