using BookStore.Models;
using BookStore.Repositories;

namespace BookStore.Services;

public class CustomerService
{
    private readonly Repository<Customer> _repo;

    public CustomerService(Repository<Customer> repo)
    {
        _repo = repo;
    }

    public (bool ok, string error) Register(Customer customer)
    {
        bool taken = _repo.Find(c => c.Email.Equals(customer.Email, StringComparison.OrdinalIgnoreCase)).Any();
        if (taken)
            return (false, "That email address is already registered.");

        _repo.Add(customer);
        return (true, string.Empty);
    }

    public Customer? FindById(int id) => _repo.GetById(id);

    public IReadOnlyList<Customer> GetAll() => _repo.GetAll();
}
