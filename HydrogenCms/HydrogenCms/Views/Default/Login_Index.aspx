<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Default/Site.Master" AutoEventWireup="true" CodeBehind="Login_Index.aspx.cs" Inherits="HydrogenCms.Views.Default.Login_Index" %>
<asp:Content ID="default" ContentPlaceHolderID="chpDefault" runat="server">
<h2>Login</h2>
<%
	if (this.TempData["loginError"] != null)
	{
		%>
		<div class="errorMessageBox">
		<%= this.TempData["loginError"] %>
		</div>		
		<%	
	}
%>
<% using (Html.Form("Login", "Login", FormMethod.Post)) { %>
Username:<br />
<%= Html.TextBox("userid", string.Empty, new { size = 20, maxlength = 35 })%><br />
Password:<br />
<%= Html.Password("password", new { maxlength = 20 })%><br />
<%= Html.SubmitButton(string.Empty, "Login") %>
<%= Html.Hidden("ReturnUrl", (Request.QueryString["ReturnUrl"] == null ? (Request.Form["ReturnUrl"] == null ? (this.TempData["ReturnUrl"] == null ? string.Empty : this.TempData["ReturnUrl"].ToString()) : Request.Form["ReturnUrl"]) : Request.QueryString["ReturnUrl"])) %>
<%} %>
</asp:Content>
