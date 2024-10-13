﻿namespace WebMarketplace;

public static class WebMarketplaceDomainErrorCodes
{
    /* You can add your business exception error codes here, as constants */
    public const string CompanyNameAlreadyExists = "Exception:CompanyNameAlreadyExists"; 
    public const string CompanyDisplayNameAlreadyExists = "Exception:CompanyDisplayNameAlreadyExists"; 
    public const string CompanyIdentificationNumberAlreadyExists = "Exception:CompanyIdentificationNumberAlreadyExists"; 
    public const string CompanyImageNotFound = "Exception:CompanyImageNotFound";
    public const string CompanyImagesNotFound = "Exception:CompanyImagesNotFound";
    public const string CompanyImageDefaultRemoveNotAllowed = "Exception:CompanyImageDefaultRemoveNotAllowed";
    public const string CompanyNotFoundForUser = "Exception:CompanyNotFoundForUser";

    public const string ProductReviewUserAlreadyExists = "Exception:ProductReviewUserAlreadyExists";
    public const string ProductReviewNotFound = "Exception:ProductReviewNotFound";
    public const string ProductImageNotFound = "Exception:ProductImageNotFound";
    public const string ProductImageDefaultRemoveNotAllowed = "Exception:ProductImageDefaultRemoveNotAllowed";
    public const string ProductCategoryUndefinedNotAllowed = "Exception:ProductCategoryUndefinedNotAllowed";
    public const string ProductTypeUndefinedNotAllowed = "Exception:ProductTypeUndefinedNotAllowed";
    public const string ProductPriceNotFound = "Exception:ProductPriceNotFound";
    
    public const string PriceNotNegative = "Exception:PriceNotNegative"; 
    public const string CurrencyAlreadySet = "Exception:CurrencyAlreadySet";
    public const string CurrencyNotFound = "Exception:CurrencyNotFound";
    public const string ImageContentIsEmpty = "Exception:ImageContentIsEmpty";

}
