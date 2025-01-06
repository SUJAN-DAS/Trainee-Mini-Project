using Microsoft.Practices.EnterpriseLibrary.Data;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Assessment_Service
{
    public class Common
    {
        public static void BindDataValidationForExcel(Database db,  ExcelWorksheet worksheet, ExcelPackage package, int columnIndex, string enumSheetName, string enumType, int minValue, int maxValue, string columnName, string promptTitle, string promptMsg, string errorTitle, string errorMsg, bool allowBlank, bool allowNewEntries, ExcelDataValidation dataValidationType)
        {
            try
            {
                string errorTitleData = string.Empty;
                string errorMsgData = string.Empty;
                string promptTitleData = string.Empty;
                string promptMsgData = string.Empty;

                switch (dataValidationType)
                {
                    case ExcelDataValidation.EnumType:
                        #region Enum List Binding

                        errorTitleData = "Error";
                        errorMsgData = "Select from list";
                        promptTitleData = "";
                        promptMsgData = "";

                        if (!string.IsNullOrEmpty(errorTitle))
                            errorTitleData = errorTitle;

                        if (!string.IsNullOrEmpty(errorMsg))
                            errorMsgData = errorMsg;

                        if (!string.IsNullOrEmpty(promptTitle))
                            promptTitleData = promptTitle;

                        if (!string.IsNullOrEmpty(promptMsg))
                            promptMsgData = promptMsg;

                        var enumTypeCell = worksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, columnIndex, ExcelPackage.MaxRows, columnIndex));
                        ExcelWorksheet workSheetCell = package.Workbook.Worksheets.Add(enumSheetName);
                        workSheetCell.Hidden = eWorkSheetHidden.Hidden;//hiding the hidden Sheet from user

                        int enumTypeIndex = 1;

                        //EnumTypeValueFilterInfo enumTypeValueFilterInfo = new EnumTypeValueFilterInfo();
                        //enumTypeValueFilterInfo.SiteID = excelValidationInfo.SiteID;
                        //enumTypeValueFilterInfo.UserID = excelValidationInfo.UserID;
                        //enumTypeValueFilterInfo.LanguageCode = excelValidationInfo.LanguageCode;
                        //enumTypeValueFilterInfo.AccessLevelID = excelValidationInfo.AccessLevelID;
                        //enumTypeValueFilterInfo.CompanyID = excelValidationInfo.CompanyID;
                        //enumTypeValueFilterInfo.EnumTypeList.Add(enumType);

                        //List<EnumTypeValueInfo> enumTypeInfoList = EnumTypeInfo(db, enumTypeValueFilterInfo);
                        //foreach (EnumTypeValueInfo enumTypeInfo in enumTypeInfoList)
                        //{
                        //    workSheetCell.Cells[enumTypeIndex, 1].Value = enumTypeInfo.DisplayName;
                        //    enumTypeIndex++;
                        //}
                        //string enumTypes = "=" + workSheetCell.ToString() + "!" + "$A$1:$A$" + enumTypeIndex.ToString();
                        //enumTypeCell.Formula.ExcelFormula = enumTypes;
                        if (!allowNewEntries)
                        {
                            enumTypeCell.ShowErrorMessage = true;
                            enumTypeCell.ErrorTitle = errorTitleData;
                            enumTypeCell.Error = errorMsgData;
                        }
                        if (allowBlank)
                        {
                            enumTypeCell.AllowBlank = true;
                        }
                        else
                        {
                            enumTypeCell.AllowBlank = false;
                        }

                        if (!string.IsNullOrEmpty(promptMsgData))
                        {
                            enumTypeCell.ShowInputMessage = true;
                            enumTypeCell.Prompt = promptMsgData;
                        }

                        break;
                    #endregion
                    case ExcelDataValidation.CustomList:
                        #region Enum List Binding

                        errorTitleData = "Error";
                        errorMsgData = "Please select option from list";
                        promptTitleData = "";
                        promptMsgData = "";

                        if (!string.IsNullOrEmpty(errorTitle))
                            errorTitleData = errorTitle;

                        if (!string.IsNullOrEmpty(errorMsg))
                            errorMsgData = errorMsg;

                        if (!string.IsNullOrEmpty(promptTitle))
                            promptTitleData = promptTitle;

                        if (!string.IsNullOrEmpty(promptMsg))
                            promptMsgData = promptMsg;

                        var customListTypeCell = worksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, columnIndex, ExcelPackage.MaxRows, columnIndex));
                        ExcelWorksheet customListWorkSheetCell = package.Workbook.Worksheets.Add(enumSheetName);

                        customListWorkSheetCell.Hidden = eWorkSheetHidden.Hidden;//hiding the hidden Sheet from user

                        int customListTypeIndex = 1;

                        //foreach (string listInfo in excelValidationInfo.CustomListOptions)
                        //{
                        //    customListWorkSheetCell.Cells[customListTypeIndex, 1].Value = listInfo;
                        //    customListTypeIndex++;
                        //}
                        //string customListTypes = "=" + customListWorkSheetCell.ToString() + "!" + "$A$1:$A$" + customListTypeIndex.ToString();
                        //customListTypeCell.Formula.ExcelFormula = customListTypes;
                        if (!allowNewEntries)
                        {
                            customListTypeCell.ShowErrorMessage = true;
                            customListTypeCell.ErrorTitle = errorTitleData;
                            customListTypeCell.Error = errorMsgData;
                        }
                        if (allowBlank)
                        {
                            customListTypeCell.AllowBlank = true;
                        }
                        else
                        {
                            customListTypeCell.AllowBlank = false;
                        }

                        if (!string.IsNullOrEmpty(promptMsgData))
                        {
                            customListTypeCell.ShowInputMessage = true;
                            customListTypeCell.Prompt = promptMsgData;
                        }
                        break;
                    #endregion                   
                    case ExcelDataValidation.TextLength:
                        #region Text Length Data Validation 

                        errorTitleData = "Error";
                        errorMsgData = string.Format(columnName + " must be less than or equal to {0} characters.", maxValue);
                        promptTitleData = "";
                        promptMsgData = "";

                        if (!string.IsNullOrEmpty(errorTitle))
                            errorTitleData = errorTitle;

                        if (!string.IsNullOrEmpty(errorMsg))
                            errorMsgData = errorMsg;

                        if (!string.IsNullOrEmpty(promptTitle))
                            promptTitleData = promptTitle;

                        if (!string.IsNullOrEmpty(promptMsg))
                            promptMsgData = promptMsg;

                        var textcolumn = worksheet.DataValidations.AddTextLengthValidation(ExcelRange.GetAddress(2, columnIndex, ExcelPackage.MaxRows, columnIndex));

                        textcolumn.ShowErrorMessage = true;
                        textcolumn.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                        textcolumn.ErrorTitle = errorTitleData;
                        textcolumn.Error = errorMsgData;
                        textcolumn.Formula.Value = minValue;
                        textcolumn.Formula2.Value = maxValue;
                        if (allowBlank)
                        {
                            textcolumn.AllowBlank = true;
                        }
                        else
                        {
                            textcolumn.AllowBlank = false;
                        }

                        if (!string.IsNullOrEmpty(promptMsgData))
                        {
                            textcolumn.ShowInputMessage = true;
                            textcolumn.Prompt = promptMsgData;
                        }
                        break;
                    #endregion
                    case ExcelDataValidation.DecimalValidation:
                        #region Decimal Data Validation 

                        errorTitleData = "Invalid data error";
                        errorMsgData = "Value must be numeric";
                        promptTitleData = "";
                        promptMsgData = "";

                        if (!string.IsNullOrEmpty(errorTitle))
                            errorTitleData = errorTitle;

                        if (!string.IsNullOrEmpty(errorMsg))
                            errorMsgData = errorMsg;

                        if (!string.IsNullOrEmpty(promptTitle))
                            promptTitleData = promptTitle;

                        if (!string.IsNullOrEmpty(promptMsg))
                            promptMsgData = promptMsg;

                        var decimalcolumn = worksheet.DataValidations.AddDecimalValidation(ExcelRange.GetAddress(2, columnIndex, ExcelPackage.MaxRows, columnIndex));

                        decimalcolumn.ShowErrorMessage = true;
                        decimalcolumn.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                        decimalcolumn.ErrorTitle = errorTitleData;
                        decimalcolumn.Error = errorMsgData;
                        decimalcolumn.Operator = ExcelDataValidationOperator.greaterThanOrEqual;
                        decimalcolumn.Formula.Value = 0D;
                        if (allowBlank)
                        {
                            decimalcolumn.AllowBlank = true;
                        }
                        else
                        {
                            decimalcolumn.AllowBlank = false;
                        }

                        if (!string.IsNullOrEmpty(promptMsgData))
                        {
                            decimalcolumn.ShowInputMessage = true;
                            decimalcolumn.Prompt = promptMsgData;
                        }

                        break;
                    #endregion
                    case ExcelDataValidation.DecimalValidationWithMaxLength:
                        #region Decimal Data Validation 

                        errorTitleData = "Invalid data error";
                        errorMsgData = "Value must be numeric and less than or equal to " + maxValue + " digits";
                        promptTitleData = "";
                        promptMsgData = "";

                        if (!string.IsNullOrEmpty(errorTitle))
                            errorTitleData = errorTitle;

                        if (!string.IsNullOrEmpty(errorMsg))
                            errorMsgData = errorMsg;

                        if (!string.IsNullOrEmpty(promptTitle))
                            promptTitleData = promptTitle;

                        if (!string.IsNullOrEmpty(promptMsg))
                            promptMsgData = promptMsg;

                        var decimalcolumn2 = worksheet.DataValidations.AddDecimalValidation(ExcelRange.GetAddress(2, columnIndex, ExcelPackage.MaxRows, columnIndex));

                        decimalcolumn2.ShowErrorMessage = true;
                        decimalcolumn2.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                        decimalcolumn2.ErrorTitle = errorTitleData;
                        decimalcolumn2.Error = errorMsgData;
                        decimalcolumn2.Formula.Value = 0D;

                        string maxChar = string.Empty;
                        for (int i = 0; i < maxValue; i++)
                            maxChar += "9";
                        decimalcolumn2.Operator = ExcelDataValidationOperator.between;
                        decimalcolumn2.Formula2.Value = Convert.ToDouble(maxChar);
                        if (allowBlank)
                        {
                            decimalcolumn2.AllowBlank = true;
                        }
                        else
                        {
                            decimalcolumn2.AllowBlank = false;
                        }

                        if (!string.IsNullOrEmpty(promptMsgData))
                        {
                            decimalcolumn2.ShowInputMessage = true;
                            decimalcolumn2.Prompt = promptMsgData;
                        }
                        break;
                    #endregion
                    case ExcelDataValidation.Integer:
                        #region Integer Data Validation 

                        errorTitleData = "Invalid data error";
                        errorMsgData = string.Format(columnName + " must be between {0} and {1}.", minValue, maxValue);
                        promptTitleData = "";
                        promptMsgData = "";

                        if (!string.IsNullOrEmpty(errorTitle))
                            errorTitleData = errorTitle;

                        if (!string.IsNullOrEmpty(errorMsg))
                            errorMsgData = errorMsg;

                        if (!string.IsNullOrEmpty(promptTitle))
                            promptTitleData = promptTitle;

                        if (!string.IsNullOrEmpty(promptMsg))
                            promptMsgData = promptMsg;

                        var integercolumn = worksheet.DataValidations.AddIntegerValidation(ExcelRange.GetAddress(2, columnIndex, ExcelPackage.MaxRows, columnIndex));

                        integercolumn.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                        //integercolumn.Prompt = string.Format("Value should be between {0} and {1}", minValue, maxValue);
                        //integercolumn.ShowInputMessage = true;
                        integercolumn.ErrorTitle = errorTitleData;
                        integercolumn.Error = errorMsgData;
                        integercolumn.ShowErrorMessage = true;
                        integercolumn.Operator = ExcelDataValidationOperator.between;
                        integercolumn.Formula.Value = minValue;
                        integercolumn.Formula2.Value = maxValue;
                        if (allowBlank)
                        {
                            integercolumn.AllowBlank = true;
                        }
                        else
                        {
                            integercolumn.AllowBlank = false;
                        }

                        if (!string.IsNullOrEmpty(promptMsgData))
                        {
                            integercolumn.ShowInputMessage = true;
                            integercolumn.Prompt = promptMsgData;
                        }
                        break;
                    #endregion
                    case ExcelDataValidation.IntegerValidationWithMaxLength:
                        #region Decimal Data Validation  

                        errorTitleData = "Invalid data error";
                        errorMsgData = "Value must be numeric and less than or equal to " + maxValue + " digits";
                        promptTitleData = "";
                        promptMsgData = "";

                        if (!string.IsNullOrEmpty(errorTitle))
                            errorTitleData = errorTitle;

                        if (!string.IsNullOrEmpty(errorMsg))
                            errorMsgData = errorMsg;

                        if (!string.IsNullOrEmpty(promptTitle))
                            promptTitleData = promptTitle;

                        if (!string.IsNullOrEmpty(promptMsg))
                            promptMsgData = promptMsg;

                        var integercolumn2 = worksheet.DataValidations.AddDecimalValidation(ExcelRange.GetAddress(2, columnIndex, ExcelPackage.MaxRows, columnIndex));

                        integercolumn2.ShowErrorMessage = true;
                        integercolumn2.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                        integercolumn2.ErrorTitle = errorTitleData;
                        integercolumn2.Error = errorMsgData;
                        integercolumn2.Formula.Value = 0;

                        string maxChars = string.Empty;
                        for (int i = 0; i < maxValue; i++)
                            maxChars += "9";
                        integercolumn2.Operator = ExcelDataValidationOperator.between;
                        integercolumn2.Formula2.Value = Convert.ToDouble(maxChars);
                        if (allowBlank)
                        {
                            integercolumn2.AllowBlank = true;
                        }
                        else
                        {
                            integercolumn2.AllowBlank = false;
                        }

                        if (!string.IsNullOrEmpty(promptMsgData))
                        {
                            integercolumn2.ShowInputMessage = true;
                            integercolumn2.Prompt = promptMsgData;
                        }
                        break;
                    #endregion
                  
                   
                    default:
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        public static string CleanHTMLTags(string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(value, "<[^>]*>", "");
        }

        public enum ExcelDataValidation
        {
            EnumType = 1,
            TextLength = 2,
            DecimalValidation = 3,
            Integer = 4,
            DecimalValidationWithMaxLength = 5,
            TimeValidation = 6,
            Date = 7,
            CustomList = 8,
            IntegerValidationWithMaxLength = 9,
        }
        public static string GetErrorMessage(string ErrorMessage)
        {
            string exceptionErrorMessage = ErrorMessage;
            if (ConfigurationManager.AppSettings["EnableCustomException"] != null)
            {
                string showCustomException = ConfigurationManager.AppSettings["EnableCustomException"].ToString().ToUpper();
                if (showCustomException == "TRUE" && exceptionErrorMessage.ToUpper().Contains("ORA"))
                {
                    exceptionErrorMessage = "Failed to perform the operation";
                }
            }
            return exceptionErrorMessage;
        }


    }
}