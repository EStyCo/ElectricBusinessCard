using ElectricBusinessCard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectricBusinessCard.Pages.Home
{
    public class FeedbackFormModel(
        IHttpClientFactory _clientFactory, 
        IConfiguration _configuration) : PageModel
    {
        [BindProperty]
        public TelegramMessage MessageModel { get; set; } = new();
        public bool ShowMessage { get; set; }
        public bool IsSuccess { get; set; }
        public string StatusMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var botToken = _configuration["Telegram:BotToken"];
            var chatId = _configuration["Telegram:ChatId"];
            var clientMessage = "\n<i>��� ���������!</i>";

            if(!string.IsNullOrEmpty(MessageModel.Message))
            {
                clientMessage = "\n<i>���������: </i><code>" + MessageModel.Message + @"</code>";
            }    

            var message = @"<b>����� ������!</b>" +
                "\n\n<i>���: </i> <code>" + MessageModel.Name + @"</code>" +
                "\n<i>�������:</i> <code>" + " +7" + Uri.EscapeDataString(MessageModel.Phone) + @"</code>" + 
                clientMessage;

            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync(
                    $"https://api.telegram.org/bot{botToken}/sendMessage?" +
                    $"chat_id={chatId}&text={Uri.EscapeDataString(message)}" +
                    "&parse_mode=HTML");

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage = "���� ������ ������� ����������! � ���� �������� � ��������� �����.";
                    IsSuccess = true;
                    ShowMessage = true;
                    ModelState.Clear();
                    return Page();
                }

                StatusMessage = "������ ��� ��������. ����������, ���������� �����.";
                IsSuccess = false;
                ShowMessage = true;
                return Page();
            }
            catch
            {
                StatusMessage = "��������� ������. ����������, ���������� �����.";
                IsSuccess = false;
                ShowMessage = true;
                return Page();
            }
        }
    }
}
