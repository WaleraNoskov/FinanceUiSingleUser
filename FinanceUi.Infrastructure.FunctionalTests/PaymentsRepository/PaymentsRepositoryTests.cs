using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Entities;
using FinanceUi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.FunctionalTests.PaymentsRepository;

public class PaymentsRepositoryTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private PaymentRepository CreateRepository(AppDbContext context) =>
        new PaymentRepository(context);

    [Fact]
    public async Task CreateAsync_AddsPayment()
    {
        using var context = GetInMemoryDbContext();
        var repo = CreateRepository(context);

        var board = new Board { Id = Guid.NewGuid(), Title = "Board" };
        await context.Boards.AddAsync(board);
        await context.SaveChangesAsync();

        var dto = new BriefPaymentDto
        {
            Name = "Rent",
            Amount = 500m,
            Periodicity = Periodicity.Monthly,
            BoardId = board.Id
        };

        var id = await repo.CreateAsync(dto);
        var payment = await context.Payments.FindAsync(id);

        Assert.NotNull(payment);
        Assert.Equal(dto.Name, payment.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectPayment()
    {
        using var context = GetInMemoryDbContext();
        var board = new Board { Id = Guid.NewGuid(), Title = "Board" };
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            Name = "Rent",
            Amount = 500,
            Periodicity = Periodicity.Monthly,
            BoardId = board.Id,
            Board = board
        };
        await context.Boards.AddAsync(board);
        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();

        var repo = CreateRepository(context);
        var dto = await repo.GetByIdAsync(payment.Id);

        Assert.NotNull(dto);
        Assert.Equal("Rent", dto.Name);
        Assert.Equal("Board", dto.BoardTitle);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsFilteredAndSorted()
    {
        using var context = GetInMemoryDbContext();
        var board = new Board { Id = Guid.NewGuid(), Title = "Board" };
        await context.Boards.AddAsync(board);
        await context.Payments.AddRangeAsync(
            new Payment { Name = "Rent", Amount = 500, BoardId = board.Id, Periodicity = Periodicity.Monthly },
            new Payment { Name = "Groceries", Amount = 200, BoardId = board.Id, Periodicity = Periodicity.Weekly }
        );
        await context.SaveChangesAsync();

        var repo = CreateRepository(context);

        var result = await repo.GetAllAsync(new GetAllPaymentsDto
        {
            Filter = "rent",
            PaginationParams = new PaginationParams { Page = 1, PageSize = 10 },
            SortingParams = new SortingParams { PropertyName = "Name", IsDescending = false }
        });

        Assert.Single(result.Items);
        Assert.Equal("Rent", result.Items.First().Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPayment()
    {
        using var context = GetInMemoryDbContext();
        var board = new Board { Id = Guid.NewGuid(), Title = "Board" };
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            Name = "Old",
            Amount = 300,
            Periodicity = Periodicity.Once,
            BoardId = board.Id
        };
        await context.Boards.AddAsync(board);
        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();

        var repo = CreateRepository(context);
        var updated = await repo.UpdateAsync(new BriefPaymentDto
        {
            Id = payment.Id,
            Name = "Updated",
            Amount = 1000,
            Periodicity = Periodicity.Monthly,
            BoardId = board.Id
        });

        var newPayment = await context.Payments.FindAsync(payment.Id);
        Assert.True(updated);
        Assert.Equal("Updated", newPayment!.Name);
        Assert.Equal(1000, newPayment.Amount);
    }

    [Fact]
    public async Task DeleteAsync_RemovesPayment()
    {
        using var context = GetInMemoryDbContext();
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            Name = "ToDelete",
            Amount = 300,
            Periodicity = Periodicity.Once,
            BoardId = Guid.NewGuid()
        };
        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();

        var repo = CreateRepository(context);
        await repo.DeleteAsync(payment.Id);

        var exists = await context.Payments.AnyAsync(p => p.Id == payment.Id);
        Assert.False(exists);
    }
}