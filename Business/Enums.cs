﻿namespace HRE.Business {

    public enum AssurePayPaymentStatus {
        Success,
        Cancelled,
        Expired,
        Failure,
        Open,
        Error
    }


    public enum OrderStatus {
        WishList,           // 0
        InCart,             // 1
        Ordered,            // 2
        OrderedAndPayed,    // 3
        OnTransport,        // 4
        ReceivedByCustomer, // 5
        Cancelled,          // 6
        SentBack,           // 7
        MoneyBack,          // 8
        Unknown,            // 9
    }


    /// <summary>
    /// The type of address. Shows the association of a user with the address.
    /// Note that primaryAddress e.g. billingAddress is NOT listed in the ENUM. The primary address is directly linked to user via the user.addressId.
    /// Only addresses in the Secondary address table have a type.
    /// </summary>
    public enum AddressTypes {
        PostalAddress,
    }

    public enum PaymentType {
        iDeal = 1,
        BankTransfer = 2,
        Cash = 3
    }

    public enum EmailType {
        Normal,
        SubscriptionConfirmation,
        EmailSubscriptionConfirmation
    }

    /// <summary>
    /// The status of an audited mail.
    /// </summary>
    public enum EmailStatus {
        Unsent,
        Sent,
        SendError,
        Cancelled
    }

    /// <summary>
    /// Overview of the categories
    /// Filled in in audited mails for purposes of statistics.
    /// </summary>
    public enum EmailCategory {
        SubscriptionConfirmation,
        ProductOrderConfirmation,
        TestResult,
        Newsletter,
        NewsletterTest,
        Test,
        ResetPassword,
        EmailUnsubscription
    }


    /// <summary>
    /// The possible user roles as also present in the ASP.NET Membershop 'Roles' table.
    /// The numbers are the same as the DB ID's (primary key), the Enum equal to the string.
    /// </summary>
    public enum UserRole {
        HRE2012Deelnemer = 1,
        Admin = 2,
        HRE2012Vrijwilliger = 3,
        Geinteresseerde = 4,
        HRE2012Sponsor = 5
    }


    /// <summary>
    /// The options to send the newsletter with regards to users being subscribed to it or not.
    /// </summary>
    public enum NewsletterSubscriptionStatus {
        OnlyToMembers = 0, // Default, basically only use this!
        SpamAll = 1, // Use with caution!
        OnlyToNonMembers = 2 // OnlyToNotify, or if forgotten previously
    }


    /// <summary>
    /// The options to send the newsletter with regards to users having paid the entry fee or not.
    /// </summary>
    public enum EntryFeePaidStatus {
        All = 0,
        OnlyPaid = 1,
        OnlyNonPaid = 2
    }


    /// <summary>
    /// The options to send the newsletter with having subscribed as an Early Bird or not.
    /// </summary>
    public enum EarlyBirdStatus {
        All = 0,
        OnlyEarlyBird = 1,
        OnlyNonEarlyBird = 2
    }


    /// <summary>
    /// The options to send the newsletter with having subscribed (so very likely done) HRE 2012.
    /// </summary>
    public enum HREEventParticipantStatus {
        All = 0,
        OnlyParticipants = 1,
        OnlyNonParticipants = 2
    }
    
    
    /// <summary>
    /// The status of a logonuser.
    /// </summary>
    public enum LogonUserStatus {
        Undetermined = 0,   // Undetermined, for records created before this Enum was added or when a user with a previously confirmed email address changed it, so that it is now unconfirmed again :).
        New = 1, // A new user, with unconfirmed (or no) e-mail address.
        EmailAddressConfirmed = 2, // THe e-mail address was confirmed
        FullAccount = 3  // The user has a password
    }

}