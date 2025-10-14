using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace TestPriem
{
    [TestClass]
    public class GoodsReceiptServiceTests
    {
        [TestMethod]
        public void CreateGoodsReceipt_WithValidData_ShouldCreateReceipt()
        {
            
            var mockRepository = new Mock<IGoodsReceiptRepository>();
            var service = new GoodsReceiptService(mockRepository.Object);

            var receiptDto = new GoodsReceiptDto
            {
                ProductId = 1,
                Quantity = 10,
                UnitCost = 100.50m,
                ReceivedByStaffId = 5,
                SupplierId = 2,
                BatchNumber = "BATCH-001"
            };

            var expectedReceipt = new GoodsReceipt
            {
                ReceiptId = 1,
                ProductId = 1,
                QuantityReceived = 10,
                UnitCost = 100.50m,
                ReceiptDate = DateTime.Now,
                ReceivedByStaffId = 5,
                SupplierId = 2,
                BatchNumber = "BATCH-001"
            };

            mockRepository.Setup(repo => repo.Add(It.IsAny<GoodsReceipt>()))
                         .Returns(expectedReceipt);

          
            var result = service.CreateGoodsReceipt(receiptDto);

           
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ReceiptId);
            Assert.AreEqual(10, result.QuantityReceived);
            mockRepository.Verify(repo => repo.Add(It.IsAny<GoodsReceipt>()), Times.Once);
        }

        [TestMethod]
        public void CreateGoodsReceipt_WithZeroQuantity_ShouldThrowException()
        {
            
            var mockRepository = new Mock<IGoodsReceiptRepository>();
            var service = new GoodsReceiptService(mockRepository.Object);

            var receiptDto = new GoodsReceiptDto
            {
                ProductId = 1,
                Quantity = 0, 
                UnitCost = 100.50m,
                ReceivedByStaffId = 5,
                SupplierId = 2,
                BatchNumber = "BATCH-001"
            };

            
            Assert.ThrowsException<ArgumentException>(() => service.CreateGoodsReceipt(receiptDto));
            mockRepository.Verify(repo => repo.Add(It.IsAny<GoodsReceipt>()), Times.Never);
        }

        [TestMethod]
        public void GetGoodsReceipt_WithValidId_ShouldReturnReceipt()
        {
          
            var mockRepository = new Mock<IGoodsReceiptRepository>();
            var service = new GoodsReceiptService(mockRepository.Object);

            var expectedReceipt = new GoodsReceipt
            {
                ReceiptId = 1,
                ProductId = 1,
                QuantityReceived = 10,
                UnitCost = 100.50m,
                ReceiptDate = DateTime.Now,
                ReceivedByStaffId = 5,
                SupplierId = 2,
                BatchNumber = "BATCH-001"
            };

            mockRepository.Setup(repo => repo.GetById(1))
                         .Returns(expectedReceipt);

          
            var result = service.GetGoodsReceipt(1);

          
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ReceiptId);
            Assert.AreEqual("BATCH-001", result.BatchNumber);
            mockRepository.Verify(repo => repo.GetById(1), Times.Once);
        }

        [TestMethod]
        public void UpdateProductStock_AfterReceipt_ShouldIncreaseQuantity()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            var service = new GoodsReceiptService(mockProductService.Object);

            var product = new Product
            {
                ProductId = 1,
                ProductName = "Test Product",
                StockQuantity = 5,
                IsActive = true
            };

            var receipt = new GoodsReceipt
            {
                ProductId = 1,
                QuantityReceived = 10
            };

            mockProductService.Setup(ps => ps.GetProduct(1))
                             .Returns(product);

            // Act
            service.UpdateProductStock(receipt);

            // Assert
            Assert.AreEqual(15, product.StockQuantity); // 5 + 10
            mockProductService.Verify(ps => ps.UpdateProduct(product), Times.Once);
        }
    }

    
    public interface IGoodsReceiptRepository
    {
        GoodsReceipt Add(GoodsReceipt receipt);
        GoodsReceipt GetById(int id);
    }

    public interface IProductService
    {
        Product GetProduct(int productId);
        void UpdateProduct(Product product);
    }

    // Сервис для тестирования
    public class GoodsReceiptService
    {
        private readonly IGoodsReceiptRepository _repository;
        private readonly IProductService _productService;

        public GoodsReceiptService(IGoodsReceiptRepository repository)
        {
            _repository = repository;
        }

        public GoodsReceiptService(IProductService productService)
        {
            _productService = productService;
        }

        public GoodsReceipt CreateGoodsReceipt(GoodsReceiptDto dto)
        {
            if (dto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var receipt = new GoodsReceipt
            {
                ProductId = dto.ProductId,
                QuantityReceived = dto.Quantity,
                UnitCost = dto.UnitCost,
                ReceiptDate = DateTime.Now,
                ReceivedByStaffId = dto.ReceivedByStaffId,
                SupplierId = dto.SupplierId,
                BatchNumber = dto.BatchNumber
            };

            return _repository.Add(receipt);
        }

        public GoodsReceipt GetGoodsReceipt(int id)
        {
            return _repository.GetById(id);
        }

        public void UpdateProductStock(GoodsReceipt receipt)
        {
            var product = _productService.GetProduct(receipt.ProductId);
            product.StockQuantity += receipt.QuantityReceived;
            _productService.UpdateProduct(product);
        }
    }
}


