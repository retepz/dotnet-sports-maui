namespace Sports.Maui.Model.Helper;

using HtmlAgilityPack;
using System.Text;

public static class DocHelper
{
    public static async Task<HtmlDocument> Get(string url)
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetStreamAsync(url);
            var doc = new HtmlDocument();
            doc.Load(response, Encoding.UTF8);
            return doc;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public static List<HtmlNode> GetChildElementNodes(
        HtmlNode node,
        string elementName = null,
        string matchingInnerText = null,
        string matchingClass = null,
        bool addIfElementNameMatches = false)
    {
        var result = new List<HtmlNode>();
        foreach (var childNode in node.ChildNodes)
        {
            if (childNode.NodeType != HtmlNodeType.Element)
            {
                continue;
            }

            var elementNameHasValue = !string.IsNullOrEmpty(elementName);
            if (elementNameHasValue && !childNode.Name.Equals(elementName, StringComparison.OrdinalIgnoreCase))
            {
                result.AddRange(GetChildElementNodes(childNode, elementName, matchingInnerText, matchingClass, addIfElementNameMatches));
                continue;
            }

            var innerTextMatches = !string.IsNullOrEmpty(matchingInnerText) && childNode.InnerText.Contains(matchingInnerText, StringComparison.OrdinalIgnoreCase);
            var classMatches = !string.IsNullOrEmpty(matchingClass) && childNode.HasClass(matchingClass);
            if (innerTextMatches || classMatches)
            {
                result.Add(childNode);
            }
            else if (elementNameHasValue && addIfElementNameMatches)
            {
                result.Add(childNode);
            }

            result.AddRange(GetChildElementNodes(childNode, elementName, matchingInnerText, matchingClass, addIfElementNameMatches));
        }


        return result;
    }

    public static List<HtmlNode> GetChildElementByDataAttribute(HtmlNode node, string expectedAttribute, string expectedElementName)
    {
        var result = new List<HtmlNode>();
        foreach (var childNode in node.ChildNodes)
        {
            if (childNode.NodeType != HtmlNodeType.Element)
            {
                continue;
            }

            if (!childNode.Name.Equals(expectedElementName, StringComparison.OrdinalIgnoreCase))
            {
                result.AddRange(GetChildElementByDataAttribute(childNode, expectedAttribute, expectedElementName));
                continue;
            }

            var matchingDataAttribute = childNode.GetDataAttribute(expectedAttribute);
            if (matchingDataAttribute == null)
            {
                result.AddRange(GetChildElementByDataAttribute(childNode, expectedAttribute, expectedElementName));
                continue;
            }

            result.Add(childNode);
            result.AddRange(GetChildElementByDataAttribute(childNode, expectedAttribute, expectedElementName));
        }

        return result;
    }

    public static List<string> GetChildAttributes(HtmlNode node, string expectedAttribute)
    {
        var results = new List<string>();
        var nodeAttribute = GetAttributeFromNode(node, expectedAttribute);
        if (!string.IsNullOrEmpty(nodeAttribute))
        {
            results.Add(nodeAttribute);
        }

        foreach (var child in node.ChildNodes)
        {
            var childExpectedAttribute = GetAttributeFromNode(node, expectedAttribute);
            if (!string.IsNullOrEmpty(childExpectedAttribute))
            {
                results.Add(childExpectedAttribute);
            }

            var childRecursiveResult = GetChildAttributes(child, expectedAttribute);
            if (childRecursiveResult.Any())
            {
                results.AddRange(childRecursiveResult);
            }
        }

        return results;
    }

    public static string GetAttributeFromNode(HtmlNode node, string attribute)
    {

        var nodeDataStreamAttribute = node.Attributes.FirstOrDefault(a =>
            a.Name.Equals(attribute, StringComparison.CurrentCultureIgnoreCase));

        return nodeDataStreamAttribute?.Value;
    }

    public static HtmlDocument GetFromHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        return doc;
    }
}
