<%@ Page Language="C#" MasterPageFile="~/Views/Default/Site.Master" AutoEventWireup="true" CodeBehind="Page_Show.aspx.cs" Inherits="HydrogenCms.Views.Default.Page_Show" %>
<asp:Content ID="default" ContentPlaceHolderID="chpDefault" runat="server">
	<h2><%= Html.Encode(this.ViewData.Model.Page.Title) %></h2>
	<%= this.ViewData.Model.Page.Content %>
</asp:Content>
