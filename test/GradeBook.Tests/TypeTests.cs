using System;
using Xunit;

namespace GradeBook.Tests;

public delegate string WriteLogDelegate(string logMessage);

public class TypeTests
{
    int count = 0;
    
    InMemoryBook GetBook(string name) {
        return new InMemoryBook(name);
    }

    private void SetName(InMemoryBook book, string name) {
        book.SetName(name);
    }

    private void GetBookSetName(InMemoryBook book, string name) {
        book = new InMemoryBook(name);
        book.SetName(name);
    }

    private void GetBookSetName(ref InMemoryBook book, string name) { 
        // Can swap 'ref' for 'out' out must be assigned to
        book = new InMemoryBook(name);
    }

    private void SetInt(ref int x) {
        x = 42;
    }

    private int GetInt() {
        return 3;
    }

    private string MakeUpperCase(string parameter) {
        return parameter.ToUpper(); // Returns a copy of the string converted to upper case
    }

    public string ReturnMessage(string message) {
        count++;
        return message;
    }

    public string IncrementCount(string message) {
        count++;
        return message.ToLower();
    }

    [Fact]
    public void GetBookReturnsDifferentObjects()
    {
        // arrange
        var book1 = GetBook("Book 1");
        var book2 = GetBook("Book 2");
        // act

        // assert
        Assert.Equal("Book 1", book1.GetName());
        Assert.Equal("Book 2", book2.GetName());
        Assert.NotSame(book1, book2);
    }

    [Fact]
    public void TwoVarsCanReferenceSameObject() {
        // arrange
        var book1 = GetBook("Book 1");
        var book2 = book1;

        // act

        // assert
        Assert.Same(book1, book2);
        Assert.True(Object.ReferenceEquals(book1, book2));
    }

    [Fact]
    public void CanSetNameFromReference() {
        // arrange
        var book1 = GetBook("Book 1");
        SetName(book1, "New Name");

        // act
        
        // assert
        Assert.Equal("New Name", book1.GetName());
    }

    [Fact]
    public void CSharpIsPassByValue() {
        // arrage
        var book1 = GetBook("Book 1");
        GetBookSetName(book1, "New Name");

        // act

        // assert
        Assert.Equal("Book 1", book1.GetName());
    }

    [Fact]
    public void PassByRef() {
        // arrange
        var book1 = GetBook("Book 1");
        GetBookSetName(ref book1, "New Name"); 
        // Can swap 'ref' for 'out' out must be assigned to

        // act

        // assert
        Assert.Equal("New Name", book1.GetName());
    }

    [Fact]
    public void ValueTypesAlsoPassByValue() {
        // arrange
        var x = GetInt();
        SetInt(ref x);

        // act

        // assesrt
        Assert.Equal(42, x);
    }

    [Fact]
    public void StringsBehaveLikeValueTypes() {
        // arrange
        string name = "David";
        var upper = MakeUpperCase(name);

        // act

        // assert
        Assert.Equal("David", name);
        Assert.Equal("DAVID", upper);
    }

    [Fact]
    public void WriteLogDelegateCanPointToMethod() {
        WriteLogDelegate log = ReturnMessage;
        //log = new WriteLogDelegate(ReturnMessage);
        log += ReturnMessage;
        log += IncrementCount;

        var result = log("Hello");
        Assert.Equal(3, count);
    }
}