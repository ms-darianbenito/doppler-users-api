using Doppler.UsersApi.Model;
using Doppler.UsersApi.Validators;
using Xunit;

namespace Doppler.UsersApi.Test
{
    public class ContactInformationValidatorTest
    {
        private readonly ContactInformationValidator _validator;

        public ContactInformationValidatorTest()
        {
            _validator = new ContactInformationValidator();
        }

        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [Theory]
        public void Validate_contact_information_should_return_error_when_firstname_is_invalid(string firstname)
        {
            var contactInformation = new ContactInformation
            {
                Firstname = firstname,
                Lastname = "lastname",
                Address = "address",
                City = "city",
                Country = "country",
                Phone = "23123",
                Province = "New York",
                ZipCode = "7688"
            };

            Assert.False(_validator.Validate(contactInformation).IsValid);
        }

        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [Theory]
        public void Validate_contact_information_should_return_error_when_lastname_is_invalid(string lastname)
        {
            var contactInformation = new ContactInformation
            {
                Firstname = "firstname",
                Lastname = lastname,
                Address = "address",
                City = "city",
                Country = "country",
                Phone = "23123",
                Province = "New York",
                ZipCode = "7688"
            };

            Assert.False(_validator.Validate(contactInformation).IsValid);
        }

        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [Theory]
        public void Validate_contact_information_should_return_error_when_address_is_invalid(string address)
        {
            var contactInformation = new ContactInformation
            {
                Firstname = "firstname",
                Lastname = "lastname",
                Address = address,
                City = "city",
                Country = "country",
                Phone = "23123",
                Province = "New York",
                ZipCode = "7688"
            };

            Assert.False(_validator.Validate(contactInformation).IsValid);
        }

        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [Theory]
        public void Validate_contact_information_should_return_error_when_city_is_invalid(string city)
        {
            var contactInformation = new ContactInformation
            {
                Firstname = "firstname",
                Lastname = "lastname",
                Address = "address",
                City = city,
                Country = "country",
                Phone = "23123",
                Province = "New York",
                ZipCode = "7688"
            };

            Assert.False(_validator.Validate(contactInformation).IsValid);
        }

        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [Theory]
        public void Validate_contact_information_should_return_error_when_country_is_invalid(string country)
        {
            var contactInformation = new ContactInformation
            {
                Firstname = "firstname",
                Lastname = "lastname",
                Address = "address",
                City = "city",
                Country = country,
                Phone = "23123",
                Province = "New York",
                ZipCode = "7688"
            };

            Assert.False(_validator.Validate(contactInformation).IsValid);
        }

        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [Theory]
        public void Validate_contact_information_should_return_error_when_phone_is_invalid(string phone)
        {
            var contactInformation = new ContactInformation
            {
                Firstname = "firstname",
                Lastname = "lastname",
                Address = "address",
                City = "city",
                Country = "country",
                Phone = phone,
                Province = "New York",
                ZipCode = "7688"
            };

            Assert.False(_validator.Validate(contactInformation).IsValid);
        }

        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [Theory]
        public void Validate_contact_information_should_not_return_error_when_zipcode_is_empty(string zipcode)
        {
            var contactInformation = new ContactInformation
            {
                Firstname = "firstname",
                Lastname = "lastname",
                Address = "address",
                City = "city",
                Country = "country",
                Phone = "23123",
                Province = "New York",
                ZipCode = zipcode,
                IdSecurityQuestion = "1",
                AnswerSecurityQuestion = "answer"
            };

            Assert.True(_validator.Validate(contactInformation).IsValid);
        }

        // TODO: When task DAT-525 is deployed in prod, we need to add the next validator
        //[InlineData(null)]
        //[InlineData(" ")]
        //[InlineData("")]
        //[Theory]
        //public void Validate_contact_information_should_return_an_error_when_AnswerSecurityQuestion_is_empty(string answer)
        //{
        //    var contactInformation = new ContactInformation
        //    {
        //        Firstname = "firstname",
        //        Lastname = "lastname",
        //        Address = "address",
        //        City = "city",
        //        Country = "country",
        //        Phone = "23123",
        //        Province = "New York",
        //        ZipCode = "7600",
        //        IdSecurityQuestion = "1",
        //        AnswerSecurityQuestion = answer
        //    };

        //    Assert.False(_validator.Validate(contactInformation).IsValid);
        //}

        [Fact]
        public void Validate_contact_information_should_return_is_valid_when_data_are_correctly()
        {
            var contactInformation = new ContactInformation
            {
                Firstname = "firstname",
                Lastname = "lastname",
                Address = "address",
                City = "city",
                Country = "country",
                Phone = "23123",
                Province = "New York",
                ZipCode = "7688",
                AnswerSecurityQuestion = "TEST",
                IdSecurityQuestion = "2",
                Company = "company",
                Industry = "1"
            };

            Assert.True(_validator.Validate(contactInformation).IsValid);
        }
    }
}
