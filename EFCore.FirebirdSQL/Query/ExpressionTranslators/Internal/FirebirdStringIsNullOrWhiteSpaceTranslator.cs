/*                 
 *     EntityFrameworkCore.FirebirdSqlSQL  - Congratulations EFCore Team
 *              https://www.FirebirdSqlsql.org/en/net-provider/ 
 *     Permission to use, copy, modify, and distribute this software and its
 *     documentation for any purpose, without fee, and without a written
 *     agreement is hereby granted, provided that the above copyright notice
 *     and this paragraph and the following two paragraphs appear in all copies. 
 * 
 *     The contents of this file are subject to the Initial
 *     Developer's Public License Version 1.0 (the "License");
 *     you may not use this file except in compliance with the
 *     License. You may obtain a copy of the License at
 *     http://www.FirebirdSqlsql.org/index.php?op=doc&id=idpl
 *
 *     Software distributed under the License is distributed on
 *     an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either
 *     express or implied.  See the License for the specific
 *     language governing rights and limitations under the License.
 *
 *              Copyright (c) 2017 Rafael Almeida
 *         Made In Sergipe-Brasil - ralms@ralms.net 
 *                  All Rights Reserved.
 */

using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Utilities;
using System;
using Microsoft.EntityFrameworkCore.Query.Sql.Internal;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
     

    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class FirebirdSqlStringIsNullOrWhiteSpaceTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _methodInfo
            = typeof(string).GetRuntimeMethod(nameof(string.IsNullOrWhiteSpace), new[] { typeof(string) });

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        ///  



        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method.Equals(_methodInfo))
            {
                var argument = methodCallExpression.Arguments[0];

                return Expression.MakeBinary(
                    ExpressionType.OrElse,
                    new IsNullExpression(argument),
                    Expression.Equal(
                        new SqlFunctionExpression(
                            "LTRIM",
                            typeof(string),
                            new[]
                            {
                                new SqlFunctionExpression(
                                    "RTRIM",
                                    typeof(string),
                                    new[] { argument })
                            }),
                        Expression.Constant("", typeof(string))));
            }

            return null;
        }
    }
}
