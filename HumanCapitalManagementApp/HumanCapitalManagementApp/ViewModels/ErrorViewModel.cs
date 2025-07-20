namespace HumanCapitalManagementApp.ViewModels
{
    public class ErrorViewModel
    {
        // RequestId helps identify and trace specific requests in case of errors
        public string? RequestId { get; set; }

        // Boolean property to determine if RequestId should be shown in the error view
        // It returns true only if RequestId is not null or empty
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

