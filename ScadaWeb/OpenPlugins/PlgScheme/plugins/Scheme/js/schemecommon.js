/*
 * Scheme common objects
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

/*
 * Requires:
 * nothing
 */

// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

// Compare operators enumeration
scada.scheme.CompareOperators = {
    EQUAL: 0,
    NOT_EQUAL: 1,
    LESS_THAN: 2,
    LESS_THAN_EQUAL: 3,
    GREATER_THAN: 4,
    GREATER_THAN_EQUAL: 5
};

// Logical operators enumeration
scada.scheme.LogicalOperators = {
    NONE: 0,
    AND: 1,
    OR: 2
};

// Scheme utilities
scada.scheme.utils = {
    // Compare two values using the specified operator
    compare: function (val1, val2, compareOper) {
        var CompareOperators = scada.scheme.CompareOperators;

        switch (compareOper)
        {
            case CompareOperators.EQUAL:
                return val1 == val2;
            case CompareOperators.NOT_EQUAL:
                return val1 != val2;
            case CompareOperators.LESS_THAN:
                return val1 < val2;
            case CompareOperators.LESS_THAN_EQUAL:
                return val1 <= val2;
            case CompareOperators.GREATER_THAN:
                return val1 > val2;
            case CompareOperators.GREATER_THAN_EQUAL:
                return val1 >= val2;
            default:
                return false;
        }
    },

    // Apply the logical operator to the two values
    isTrue: function (val1, val2, logicalOper) {
        var LogicalOperators = scada.scheme.LogicalOperators;

        switch (logicalOper)
        {
            case LogicalOperators.AND:
                return val1 && val2;
            case LogicalOperators.OR:
                return val1 || val2;
            default:
                return false;
        }
    },

    conditionSatisfied: function (cond, cnlVal) {
        var LogicalOperators = scada.scheme.LogicalOperators;
        var comp1 = this.compare(cnlVal, cond.CompareArgument1, cond.CompareOperator1);

        if (cond.LogicalOperator == LogicalOperators.NONE) {
            return comp1;
        } else {
            var comp2 = this.compare(cnlVal, cond.CompareArgument2, cond.CompareOperator2);
            return this.isTrue(comp1, comp2, cond.LogicalOperator);
        }
    }
}