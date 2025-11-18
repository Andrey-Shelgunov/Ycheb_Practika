using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using System.Linq;

namespace IReturnService
{
    public partial class Return: Form
    {
        public Return()
        {
            InitializeComponent();
        }
        public interface IReturnService
        {
            Task<ReturnResult> SubmitReturnAsync(ReturnRequest request);
            Task<bool> CancelReturnAsync(string returnId);
        }
        public class SimpleReturnService : IReturnService
        {
            private readonly List<ReturnRequest> _returns = new List<ReturnRequest>();

            public async Task<ReturnResult> SubmitReturnAsync(ReturnRequest request)
            {
                // Простая проверка на заполненность полей
                if (string.IsNullOrEmpty(request.OrderNumber) ||
                    string.IsNullOrEmpty(request.Reason) ||
                    string.IsNullOrEmpty(request.CustomerName) ||
                    string.IsNullOrEmpty(request.Email) ||
                    request.ProductIds.Count == 0)
                {
                    return new ReturnResult
                    {
                        Success = false,
                        Message = "Пожалуйста, заполните все поля"
                    };
                }

                // Создаем возврат
                var returnId = $"RET-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";

                _returns.Add(request);

                return new ReturnResult
                {
                    Success = true,
                    Message = "Возврат успешно оформлен",
                    ReturnId = returnId
                };
            }

            public async Task<bool> CancelReturnAsync(string returnId)
            {
                // Простая отмена - удаляем из списка
                var returnToRemove = _returns.FirstOrDefault(r => r.OrderNumber == returnId);
                if (returnToRemove != null)
                {
                    _returns.Remove(returnToRemove);
                    return true;
                }
                return false;
            }
        }
    }
}
