/*
 * Tree view control
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

/*
 * Requires:
 * - jquery
 * - scadautils.js
 */

// Rapid SCADA namespace
var scada = scada || {};

// Tree views processing object
scada.treeView = {
    // Expand or collapse tree node
    _toggleTreeNode: function (divExpander) {
        var treeNode = divExpander.parent().parent();
        var childNodes = treeNode.next(".child-nodes");

        if (divExpander.hasClass("expanded")) {
            divExpander.removeClass("expanded");
            childNodes.css("display", "none");
        } else {
            divExpander.addClass("expanded");
            childNodes.css("display", "block");
        }
    },

    // Expand selected tree node
    _expandSelectedTreeNode: function (allNodes) {
        var selNodes = allNodes.filter(".selected");
        var parDivs = selNodes.parentsUntil(".tree-view", ".child-nodes");

        parDivs.prev(".node").find(".expander").addClass("expanded");
        parDivs.css("display", "block");
    },

    // Tune tree view elements and bind events
    prepare: function () {
        var treeViews = $(".tree-view");
        var oneIndent = treeViews.find(".indent:first").width();
        var allNodes = treeViews.find(".node");

        // set width of indents according to their level
        allNodes.each(function () {
            var level = $(this).attr("data-level");
            $(this).find(".indent").width(oneIndent * level);
        });

        // expand selected tree node
        this._expandSelectedTreeNode(allNodes);

        // go to link or toggle tree node on click
        var thisTreeView = this;
        allNodes
        .off()
        .click(function (event) {
            if (event.ctrlKey || event.button == 1 /*middle*/) {
                return; // allow the default link behavior
            }

            event.preventDefault();
            var node = $(this);
            var script = node.attr("data-script");
            var expander = node.find(".expander");
            var href = node.attr("href");

            if ($(event.target).is(".expander")) {
                thisTreeView._toggleTreeNode(expander);
            } else if (script) {
                allNodes.removeClass("selected");
                node.addClass("selected");
                eval(script);
            } else if (href.length > 0) {
                allNodes.removeClass("selected");
                node.addClass("selected");
                scada.utils.clickLink(node);
            } else {
                if (!expander.hasClass("empty")) {
                    thisTreeView._toggleTreeNode(expander);
                }
            }
        });
    }
};