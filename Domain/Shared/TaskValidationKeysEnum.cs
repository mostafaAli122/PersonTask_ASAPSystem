namespace Domain.Shared
{
    public enum TaskValidationKeysEnum
    {
        NonAuthorized = 1,
        NotFound = 2,
        FileNotExist = 3,
        WaitTooLong = 4,
        UnhandeledException = 5,
        WrongUserDetails=6,
        WrongUserNameOrPassword=7,
        NameRequired=8,
        AgeRequire=9,
        AddressRequired=10,
        StreetAdressRequired = 11,
        ZipCodeRequired = 12,
        CityIdRequired = 13,

    }

}
