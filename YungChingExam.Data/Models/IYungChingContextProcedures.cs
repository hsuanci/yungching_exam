﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using YungChingExam.Data.Models;

namespace YungChingExam.Data.Models
{
    public partial interface IYungChingContextProcedures
    {
        Task<List<CustOrderHistResult>> CustOrderHistAsync(string CustomerID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<CustOrdersDetailResult>> CustOrdersDetailAsync(int? OrderID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<CustOrdersOrdersResult>> CustOrdersOrdersAsync(string CustomerID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<EmployeeSalesbyCountryResult>> EmployeeSalesbyCountryAsync(DateTime? Beginning_Date, DateTime? Ending_Date, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SalesbyYearResult>> SalesbyYearAsync(DateTime? Beginning_Date, DateTime? Ending_Date, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SalesByCategoryResult>> SalesByCategoryAsync(string CategoryName, string OrdYear, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<TenMostExpensiveProductsResult>> TenMostExpensiveProductsAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
