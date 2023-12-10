﻿using Aspire.Core.IRepository.Base;
using Aspire.Core.Model.Models;
using Autofac;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SqlSugar;
using Xunit;
using Xunit.Abstractions;

namespace Aspire.Core.Tests;

public class OrmTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IBaseRepository<AspireArticle> _baseRepository;
    DI_Test dI_Test = new DI_Test();

    public OrmTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        var container = dI_Test.DICollections();

        _baseRepository = container.Resolve<IBaseRepository<AspireArticle>>();
        _baseRepository.Db.Aop.OnLogExecuting = (sql, p) =>
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine("==================FullSql=====================", "", new string[] { sql.GetType().ToString(), GetParas(p), "【SQL语句】：" + sql });
            _testOutputHelper.WriteLine("【SQL语句】：" + sql);
            _testOutputHelper.WriteLine(GetParas(p));
            _testOutputHelper.WriteLine("==============================================");
            _testOutputHelper.WriteLine("");
        };
    }

    private static string GetParas(SugarParameter[] pars)
    {
        string key = "【SQL参数】：";
        foreach (var param in pars)
        {
            key += $"{param.ParameterName}:{param.Value}\n";
        }

        return key;
    }

    [Fact]
    public void MultiTables()
    {
        var sql = _baseRepository.Db.Queryable<AspireArticle>()
            .AS($@"{nameof(AspireArticle)}_TenantA")
            .ToSqlString();
        //_testOutputHelper.WriteLine(sql);

        _baseRepository.Db.MappingTables.Add(nameof(AspireArticle), $@"{nameof(AspireArticle)}_TenantA");

        var query = _baseRepository.Db.Queryable<AspireArticle>()
            .LeftJoin<AspireArticleComment>((a, c) => a.bID == c.bID);
        // query.QueryBuilder.AsTables.AddOrModify(nameof(AspireArticle), $@"{nameof(AspireArticle)}_TenantA");
        //query.QueryBuilder.AsTables.AddOrModify(nameof(AspireArticleComment), $@"{nameof(AspireArticleComment)}_TenantA");
        // query.QueryBuilder.AsTables.AddOrModify(nameof(AspireArticleComment), $@"{nameof(AspireArticleComment)}_TenantA");
        // query.QueryBuilder.AsTables.AddOrModify(nameof(SysUserInfo), $@"{nameof(SysUserInfo)}_TenantA");


        sql = query.ToSqlString();

        _testOutputHelper.WriteLine(sql);

        sql = _baseRepository.Db.Deleteable<AspireArticle>().ToSqlString();
        _testOutputHelper.WriteLine(sql);
    }
}