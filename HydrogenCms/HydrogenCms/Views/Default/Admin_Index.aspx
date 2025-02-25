<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Default/Site.Master" AutoEventWireup="true" CodeBehind="Admin_Index.aspx.cs" Inherits="HydrogenCms.Views.Default.Admin_Index" %>
<asp:Content ID="default" ContentPlaceHolderID="chpDefault" runat="server">
<h2>Administration</h2>
<%
	HydrogenCms.Models.ViewContainers.AdminView adminView = null;
	
	if (this.TempData["adminView"] != null)
	{
		adminView = this.TempData["adminView"] as HydrogenCms.Models.ViewContainers.AdminView;	
	}
	else
	{
		adminView = this.ViewData.Model;
	}		
		
	if (this.TempData["adminError"] != null)
	{
		%>
		<div class="errorMessageBox">
		<%= this.TempData["adminError"] %>
		</div>		
		<%	
	}
%>
<%
	if (this.TempData["adminSuccess"] != null)
	{
		%>
		<div class="successMessageBox">
		<%= this.TempData["adminSuccess"] %>
		</div>		
		<%	
	}
%>
<%= Html.ActionLink("Logout", "Logout", "Login") %><br /><br />
<% using (Html.Form("Admin", "Update", FormMethod.Post)) { %>
Media Path:<br />
<%= Html.TextBox("MediaPath", adminView.MediaPath, new { size = 30, maxlength = 128 })%><br />
Site Name:<br />
<%= Html.TextBox("SiteName", adminView.SiteName, new { size = 30, maxlength = 128 })%><br />
Site Tag Name:<br />
<%= Html.TextBox("SiteTagLine", adminView.SiteTagLine, new { size = 30, maxlength = 128 })%><br />
Skin:<br />
<%= Html.DropDownList("Skin", new SelectList(this.ViewData.Model.Skins), adminView.Skin) %><br />
User Id:<br />
<%= Html.TextBox("UserId", adminView.UserId, new { size = 30, maxlength = 128 })%><br />
Password:<br />
<%= Html.Password("Password", new { size = 30 }) %> Leave blank to not change.<br />
<%= Html.SubmitButton(string.Empty, "Update") %>
<%} %>
</asp:Content>
