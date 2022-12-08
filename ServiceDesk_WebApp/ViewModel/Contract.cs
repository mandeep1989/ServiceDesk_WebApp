using Newtonsoft.Json;

namespace ServiceDesk_WebApp.ViewModel
{
    public class Contract
    {
        public string comments { get; set; }
        public FromDate from_date { get; set; }
        public string total_price { get; set; }
        public string notify_before { get; set; }
        public ContractStatus contract_status { get; set; }
        public string esclate_to_id { get; set; }
        public bool is_escalated { get; set; }
        public ToDate to_date { get; set; }
        public string custom_contract_id { get; set; }
        public ContractVendor vendor { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public CreatedDate created_date { get; set; }
        public string support { get; set; }
        public ContractUser user { get; set; }
        public bool is_renewed { get; set; }

        public Contract()
        {
            vendor = new ContractVendor();
        }
    }
    public class ContractStatus
    {
        public string id { get; set; }
    }

    public class CreatedDate
    {
        public string display_value { get; set; }
        public string value { get; set; }
    }

    public class VendorCurrency
    {
        public string id { get; set; }
    }

    public class Department
    {
        public object site { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class FromDate
    {
        public string display_value { get; set; }
        public string value { get; set; }
    }

    public class ListInfo
    {
        public bool has_more_rows { get; set; }
        public int start_index { get; set; }
        public int row_count { get; set; }
    }

    public class ProfilePic
    {
        [JsonProperty("content-url")]
        public string contenturl { get; set; }
        public string name { get; set; }
    }

    public class ResponseStatus
    {
        public int status_code { get; set; }
        public string status { get; set; }
    }

    public class VendorRoot
    {
        public List<ResponseStatus> response_status { get; set; }
        public ListInfo list_info { get; set; }
        public List<Contract> contracts { get; set; }
    }

    public class ToDate
    {
        public string display_value { get; set; }
        public string value { get; set; }
    }

    public class ContractUser
    {
        public string email_id { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public object mobile { get; set; }
        public ProfilePic profile_pic { get; set; }
        public bool is_vipuser { get; set; }
        public string id { get; set; }
        public Department department { get; set; }
    }

    public class ContractVendor
    {
        public string name { get; set; }
        public Currency currency { get; set; }
        public string id { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CreatedTime
    {
        public string display_value { get; set; }
        public string value { get; set; }
    }

    public class Currency
    {
        public string symbol { get; set; }
        public string code { get; set; }
        public string exchange_rate { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Root
    {
        public List<Vendor> vendors { get; set; }
    }

    public class APIVendor
    {
        public string email_id { get; set; }
        public CreatedTime created_time { get; set; }
        public string country { get; set; }
        public string contact_person { get; set; }
        public string city { get; set; }
        public string mobile { get; set; }
        public string description { get; set; }
        public string web_url { get; set; }
        public string phone { get; set; }
        public string street { get; set; }
        public string name { get; set; }
        public string door_no { get; set; }
        public string location { get; set; }
        public Currency currency { get; set; }
        public string id { get; set; }
        public string res_phone { get; set; }
        public string state { get; set; }
        public string fax { get; set; }
        public string landmark { get; set; }
        public string postal_code { get; set; }
    }


}
