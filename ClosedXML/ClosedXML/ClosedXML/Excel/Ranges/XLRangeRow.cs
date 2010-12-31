﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClosedXML.Excel
{
    internal class XLRangeRow: XLRangeBase, IXLRangeRow
    {
        public XLRangeRow(XLRangeParameters xlRangeParameters): base(xlRangeParameters.RangeAddress)
        {
            Worksheet = xlRangeParameters.Worksheet;
            Worksheet.RangeShiftedRows += new RangeShiftedRowsDelegate(Worksheet_RangeShiftedRows);
            Worksheet.RangeShiftedColumns += new RangeShiftedColumnsDelegate(Worksheet_RangeShiftedColumns);
            this.defaultStyle = new XLStyle(this, xlRangeParameters.DefaultStyle);
        }

        void Worksheet_RangeShiftedColumns(XLRange range, int columnsShifted)
        {
            ShiftColumns(this.RangeAddress, range, columnsShifted);
        }
        void Worksheet_RangeShiftedRows(XLRange range, int rowsShifted)
        {
            ShiftRows(this.RangeAddress, range, rowsShifted);
        }

        public IXLCell Cell(int column)
        {
            return Cell(1, column);
        }
        public new IXLCell Cell(string column)
        {
            return Cell(1, column);
        }

        public IXLRange Range(int firstColumn, int lastColumn)
        {
            return Range(1, firstColumn, 1, lastColumn);
        }

        public void Delete()
        {
            Delete(XLShiftDeletedCells.ShiftCellsUp);
        }

        public void InsertCellsAfter(int numberOfColumns) 
        {
            InsertColumnsAfter(numberOfColumns);
        }

        public void InsertCellsBefore(int numberOfColumns)
        {
            InsertColumnsBefore(numberOfColumns);
        }

        public IXLCells Cells(String cellsInRow)
        {
            var retVal = new XLCells(Worksheet);
            var rangePairs = cellsInRow.Split(',');
            foreach (var pair in rangePairs)
            {
                retVal.AddRange(Range(pair.Trim()).Cells());
            }
            return retVal;
        }

        public override IXLRange Range(String rangeAddressStr)
        {
            String rangeAddressToUse;
            if (rangeAddressStr.Contains(":"))
            {
                String[] arrRange = rangeAddressStr.Split(':');
                var firstPart = arrRange[0];
                var secondPart = arrRange[1];
                rangeAddressToUse = FixRowAddress(firstPart) + ":" + FixRowAddress(secondPart);
            }
            else
            {
                rangeAddressToUse = FixRowAddress(rangeAddressStr);
            }

            var rangeAddress = new XLRangeAddress(rangeAddressToUse);
            return Range(rangeAddress);
        }

        public IXLCells Cells(Int32 firstColumn, Int32 lastColumn)
        {
            return Cells(firstColumn + ":" + lastColumn);
        }

        public IXLCells Cells(String firstColumn, String lastColumn)
        {
            return Cells(XLAddress.GetColumnNumberFromLetter(firstColumn) + ":"
                + XLAddress.GetColumnNumberFromLetter(lastColumn));
        }
    }
}

