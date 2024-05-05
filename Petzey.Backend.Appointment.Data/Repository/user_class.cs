using System.Collections.Generic;

public class user_class
{
    public string GetRoleFromAdditionalData(IDictionary<string, object> additionalData)
    {
        if (additionalData.TryGetValue("extension_c4e1a20ccccd48028cb0394512be2f98_role", out var roleValue))
        {
            if (roleValue is string role)
            {
                return role;
            }
        }

        return ""; // Default value if role is not found
    }

    public string GetNpiFromAdditionalData(IDictionary<string, object> additionalData)
    {
        if (additionalData.TryGetValue("extension_c4e1a20ccccd48028cb0394512be2f98_NPINumber", out var NpiValue))
        {
            if (NpiValue is string Npi)
            {
                return Npi;
            }
        }

        return ""; // Default value if npi is not found
    }

    public string GetPhoneFromAdditionalData(IDictionary<string, object> additionalData)
    {
        if (additionalData.TryGetValue("extension_c4e1a20ccccd48028cb0394512be2f98_PhoneNumber", out var PhoneValue))
        {
            if (PhoneValue is string Phone)
            {
                return Phone;
            }
        }

        return ""; // Default value if phone number is not found
    }

    public string GetSpecialityFromAdditionalData(IDictionary<string, object> additionalData)
    {
        if (additionalData.TryGetValue("extension_c4e1a20ccccd48028cb0394512be2f98_Speciality", out var SpecialityValue))
        {
            if (SpecialityValue is string sp)
            {
                return sp;
            }
        }

        return ""; // Default value if speciality is not found
    }
}

