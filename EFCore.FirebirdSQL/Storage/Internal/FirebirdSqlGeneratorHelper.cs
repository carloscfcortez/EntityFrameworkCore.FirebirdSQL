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

using System;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.Update;


namespace Microsoft.EntityFrameworkCore.Storage.Internal
{
    public class FirebirdSqlSqlGenerationHelper : RelationalSqlGenerationHelper
    {
        public FirebirdSqlSqlGenerationHelper([NotNull] RelationalSqlGenerationHelperDependencies dependencies)
            : base(dependencies)
        {
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override string EscapeIdentifier(string identifier)
            => Check.NotEmpty(identifier, nameof(identifier));

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override void EscapeIdentifier(StringBuilder builder, string identifier)
        {
            Check.NotEmpty(identifier, nameof(identifier));

            var initialLength = builder.Length;
            builder.Append(identifier);
            //builder.Replace("", "", initialLength, identifier.Length);
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override string DelimitIdentifier(string identifier)
            => $"\"{EscapeIdentifier(Check.NotEmpty(identifier, nameof(identifier)))}\""; // Interpolation okay; strings
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override void DelimitIdentifier(StringBuilder builder, string identifier)
        {
            Check.NotEmpty(identifier, nameof(identifier));
            builder.Append('"');
            EscapeIdentifier(builder, identifier);
            builder.Append('"');
        }

        //
        // Summary:
        //     Generates a valid parameter name for the given candidate name.
        //
        // Parameters:
        //   name:
        //     The candidate name for the parameter.
        //
        // Returns:
        //     A valid name based on the candidate name.
        public override string GenerateParameterName(string name)
        {
            return "@" + name;
        }

        //
        // Summary:
        //     Writes a valid parameter name for the given candidate name.
        //
        // Parameters:
        //   builder:
        //     The System.Text.StringBuilder to write generated string to.
        //
        //   name:
        //     The candidate name for the parameter.
        public override void GenerateParameterName(StringBuilder builder, string name)
        {
            builder.Append("@").Append(name);
        }

        public static object  GenerateValue(ColumnModification column)
        {
            object value = null;
            if (column.Property.ClrType == typeof(string))
                value = ($"'{column.Value}'");
            else if (column.Property.ClrType == typeof(int)
                 || column.Property.GetType() == typeof(int?)
                 || column.Property.GetType() == typeof(long)
                 || column.Property.GetType() == typeof(long?)
                 )
                value = (column.Value);
            else if (column.Property.ClrType == typeof(decimal)
               || column.Property.GetType() == typeof(decimal?)
               || column.Property.GetType() == typeof(double)
               || column.Property.GetType() == typeof(double?)
           )
                value = (column.Value);
            else if (column.Property.ClrType == typeof(DateTime)
              || column.Property.GetType() == typeof(DateTime?)
              || column.Property.GetType() == typeof(TimeSpan)
              || column.Property.GetType() == typeof(TimeSpan?)
          )
                value = ((column.Value == null ? null : $"'{DateTime.Parse(column.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss.ffff")}'"));

            else
                value = ($"'{column.Value}'");

            return value;
        }

        public  static  void GenerateValue(StringBuilder builder,ColumnModification column)
        { 
           builder.Append(GenerateValue(column));
        }
    }
}
