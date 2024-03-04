namespace User_Management_System_Implementation.SD
{
    public static class SDRoutes
    {
        public const string baseUrl = "api";

        public const string Management = baseUrl + "/management";

        public const string Individual = baseUrl + "/individual";

        public const string Emails = baseUrl + "/emails";

        public const string Sms = baseUrl + "/sms";


        public const string UserRole =   "userrole" ;

        public const string Route =  "route" ;

        public const string RouteAccess =  "routeaccess" ;

        public const string Menu =  "menu" ;

        public const string MenuAccess =  "menuaccess" ;

        public const string Item =  "item" ;

        public const string Service =  "service" ;

        public const string ConfigureService =  "configureservice" ;

        public const string User = "user" ;

        public const string GetAll = "/getall";

        public const string Get = "/get";

        public const string GetBy = "getby";


        public const string MenusWithSubMenus = Get + "menuswithsubmenus";


        public const string Authenticate = "/authenticate";

        public const string Register = "/register";

        public const string Create = "/create";


        public const string Update = "/update";

        public const string Upsert = "upsert";

        public const string UpSertRoleAccess = Upsert + "roleaccess";

        public const string UpSertMenuAccess = Upsert + "menuaccess";

        public const string UpSertRouteAccess = Upsert + "routeaccess";

        public const string ActivateDeactivateUser = "/activatedeactivateuser";

        public const string Delete = "/delete";

        public const string ResetPassword = "/resetpassword";

        public const string UpdatePassword = "/updatepassword";

        public const string OutlookSmtp = "outlooksmtp";

        public const string ElasticMail = "elasticmail";

        public const string SendGrid = "sendgrid";

        public const string AwsSimpleEmail = "awssimpleemail";

        public const string PostMark = "postmark";


        public const string Twilio = "twilio";

        public const string Nexmo = "nexmo";

        public const string AwsSimpleNotifiation = "awssimplenotifiation";

        public const string VerifyEmailsAndMessages = "verifyemailsandmessages";


    }
}
