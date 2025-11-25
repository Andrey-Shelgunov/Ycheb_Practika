using Library;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IReturnService
{
    partial class Return
    {
        public partial class ReturnForm : Form
        {
            private readonly IReturnService _returnService;
            private TextBox txtOrderNumber;
            private TextBox txtCustomerName;
            private TextBox txtEmail;
            private TextBox txtProductIds;
            private ComboBox cmbReason;
            private Button btnSubmit;
            private Button btnCancel;
            private Label lblError;

            public ReturnForm(IReturnService returnService)
            {
                _returnService = returnService;
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.Text = "Оформление возврата";
                this.Size = new Size(400, 400);
                this.StartPosition = FormStartPosition.CenterScreen;

                // Поле номера заказа
                var lblOrderNumber = new Label { Text = "Номер заказа:", Location = new Point(20, 20), Width = 150 };
                txtOrderNumber = new TextBox { Location = new Point(170, 20), Width = 200 };

                // Поле имени клиента
                var lblCustomerName = new Label { Text = "Ваше имя:", Location = new Point(20, 60), Width = 150 };
                txtCustomerName = new TextBox { Location = new Point(170, 60), Width = 200 };

                // Поле email
                var lblEmail = new Label { Text = "Email:", Location = new Point(20, 100), Width = 150 };
                txtEmail = new TextBox { Location = new Point(170, 100), Width = 200 };

                // Поле товаров для возврата
                var lblProductIds = new Label { Text = "ID товаров (через запятую):", Location = new Point(20, 140), Width = 150 };
                txtProductIds = new TextBox { Location = new Point(170, 140), Width = 200 };

                // Причина возврата
                var lblReason = new Label { Text = "Причина возврата:", Location = new Point(20, 180), Width = 150 };
                cmbReason = new ComboBox { Location = new Point(170, 180), Width = 200 };
                cmbReason.Items.AddRange(new[] { "Не понравился", "Дефект", "Перепутанный заказ", "Другая причина" });

                // Кнопки
                btnSubmit = new Button { Text = "Отправить", Location = new Point(100, 250), Width = 80 };
                btnCancel = new Button { Text = "Отмена", Location = new Point(200, 250), Width = 80 };

                // Сообщение об ошибке
                lblError = new Label { Text = "", Location = new Point(20, 300), Width = 350, ForeColor = Color.Red };

                // Добавление элементов на форму
                this.Controls.AddRange(new Control[] {
            lblOrderNumber, txtOrderNumber,
            lblCustomerName, txtCustomerName,
            lblEmail, txtEmail,
            lblProductIds, txtProductIds,
            lblReason, cmbReason,
            btnSubmit, btnCancel,
            lblError
        });

                // Обработчики событий
                btnSubmit.Click += async (s, e) => await SubmitReturn();
                btnCancel.Click += (s, e) => this.Close();
            }

            private async Task SubmitReturn()
            {
                // Скрываем предыдущие ошибки
                lblError.Text = "";
                lblError.Visible = false;

                // Парсим ID товаров
                var productIds = new List<int>();
                if (!string.IsNullOrEmpty(txtProductIds.Text))
                {
                    foreach (var idStr in txtProductIds.Text.Split(','))
                    {
                        if (int.TryParse(idStr.Trim(), out int productId))
                        {
                            productIds.Add(productId);
                        }
                    }
                }

                // Создаем запрос
                var request = new ReturnRequest
                {
                    OrderNumber = txtOrderNumber.Text,
                    CustomerName = txtCustomerName.Text,
                    Email = txtEmail.Text,
                    ProductIds = productIds,
                    Reason = cmbReason.SelectedItem?.ToString() ?? ""
                };

                // Отправляем запрос
                var result = await _returnService.SubmitReturnAsync(request);

                if (result.Success)
                {
                    MessageBox.Show($"Возврат оформлен!\nНомер возврата: {result.ReturnId}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    // Показываем ошибку
                    lblError.Text = result.Message;
                    lblError.Visible = true;
                }

            }
        }
    }
}

