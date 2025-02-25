﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HydrogenCms.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Runtime.Serialization;
	using System.ComponentModel;
	using System;
	
	
	public partial class DataContext : System.Data.Linq.DataContext
	{
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertSetting(Setting instance);
    partial void UpdateSetting(Setting instance);
    partial void DeleteSetting(Setting instance);
    partial void InsertPage(Page instance);
    partial void UpdatePage(Page instance);
    partial void DeletePage(Page instance);
    partial void InsertMeta(Meta instance);
    partial void UpdateMeta(Meta instance);
    partial void DeleteMeta(Meta instance);
    #endregion
		
		public DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Setting> Settings
		{
			get
			{
				return this.GetTable<Setting>();
			}
		}
		
		public System.Data.Linq.Table<Page> Pages
		{
			get
			{
				return this.GetTable<Page>();
			}
		}
		
		public System.Data.Linq.Table<Meta> Metas
		{
			get
			{
				return this.GetTable<Meta>();
			}
		}
	}
	
	[DataContract()]
	public partial class Setting : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _SettingId;
		
		private string _Name;
		
		private string _Value;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSettingIdChanging(int value);
    partial void OnSettingIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnValueChanging(string value);
    partial void OnValueChanged();
    #endregion
		
		public Setting()
		{
			this.Initialize();
		}
		
		[DataMember(Order=1)]
		public int SettingId
		{
			get
			{
				return this._SettingId;
			}
			set
			{
				if ((this._SettingId != value))
				{
					this.OnSettingIdChanging(value);
					this.SendPropertyChanging();
					this._SettingId = value;
					this.SendPropertyChanged("SettingId");
					this.OnSettingIdChanged();
				}
			}
		}
		
		[DataMember(Order=2)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[DataMember(Order=3)]
		public string Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				if ((this._Value != value))
				{
					this.OnValueChanging(value);
					this.SendPropertyChanging();
					this._Value = value;
					this.SendPropertyChanged("Value");
					this.OnValueChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void Initialize()
		{
			OnCreated();
		}
		
		[OnDeserializing()]
		[System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}
	}
	
	[DataContract()]
	public partial class Page : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _PageId;
		
		private System.Nullable<System.Guid> _ParentId;
		
		private string _Slug;
		
		private string _Title;
		
		private string _Content;
		
		private bool _Published;
		
		private System.DateTime _PublishDate;
		
		private int _DisplayOrder;
		
		private System.DateTime _CreatedOn;
		
		private string _CreatedBy;
		
		private System.Nullable<System.DateTime> _ModifiedOn;
		
		private string _ModifiedBy;
		
		private EntitySet<Meta> _Metas;
		
		private bool serializing;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPageIdChanging(System.Guid value);
    partial void OnPageIdChanged();
    partial void OnParentIdChanging(System.Nullable<System.Guid> value);
    partial void OnParentIdChanged();
    partial void OnSlugChanging(string value);
    partial void OnSlugChanged();
    partial void OnTitleChanging(string value);
    partial void OnTitleChanged();
    partial void OnContentChanging(string value);
    partial void OnContentChanged();
    partial void OnPublishedChanging(bool value);
    partial void OnPublishedChanged();
    partial void OnPublishDateChanging(System.DateTime value);
    partial void OnPublishDateChanged();
    partial void OnDisplayOrderChanging(int value);
    partial void OnDisplayOrderChanged();
    partial void OnCreatedOnChanging(System.DateTime value);
    partial void OnCreatedOnChanged();
    partial void OnCreatedByChanging(string value);
    partial void OnCreatedByChanged();
    partial void OnModifiedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnModifiedOnChanged();
    partial void OnModifiedByChanging(string value);
    partial void OnModifiedByChanged();
    #endregion
		
		public Page()
		{
			this.Initialize();
		}
		
		[DataMember(Order=1)]
		public System.Guid PageId
		{
			get
			{
				return this._PageId;
			}
			set
			{
				if ((this._PageId != value))
				{
					this.OnPageIdChanging(value);
					this.SendPropertyChanging();
					this._PageId = value;
					this.SendPropertyChanged("PageId");
					this.OnPageIdChanged();
				}
			}
		}
		
		[DataMember(Order=2)]
		public System.Nullable<System.Guid> ParentId
		{
			get
			{
				return this._ParentId;
			}
			set
			{
				if ((this._ParentId != value))
				{
					this.OnParentIdChanging(value);
					this.SendPropertyChanging();
					this._ParentId = value;
					this.SendPropertyChanged("ParentId");
					this.OnParentIdChanged();
				}
			}
		}
		
		[DataMember(Order=3)]
		public string Slug
		{
			get
			{
				return this._Slug;
			}
			set
			{
				if ((this._Slug != value))
				{
					this.OnSlugChanging(value);
					this.SendPropertyChanging();
					this._Slug = value;
					this.SendPropertyChanged("Slug");
					this.OnSlugChanged();
				}
			}
		}
		
		[DataMember(Order=4)]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}
			}
		}
		
		[DataMember(Order=5)]
		public string Content
		{
			get
			{
				return this._Content;
			}
			set
			{
				if ((this._Content != value))
				{
					this.OnContentChanging(value);
					this.SendPropertyChanging();
					this._Content = value;
					this.SendPropertyChanged("Content");
					this.OnContentChanged();
				}
			}
		}
		
		[DataMember(Order=6)]
		public bool Published
		{
			get
			{
				return this._Published;
			}
			set
			{
				if ((this._Published != value))
				{
					this.OnPublishedChanging(value);
					this.SendPropertyChanging();
					this._Published = value;
					this.SendPropertyChanged("Published");
					this.OnPublishedChanged();
				}
			}
		}
		
		[DataMember(Order=7)]
		public System.DateTime PublishDate
		{
			get
			{
				return this._PublishDate;
			}
			set
			{
				if ((this._PublishDate != value))
				{
					this.OnPublishDateChanging(value);
					this.SendPropertyChanging();
					this._PublishDate = value;
					this.SendPropertyChanged("PublishDate");
					this.OnPublishDateChanged();
				}
			}
		}
		
		[DataMember(Order=8)]
		public int DisplayOrder
		{
			get
			{
				return this._DisplayOrder;
			}
			set
			{
				if ((this._DisplayOrder != value))
				{
					this.OnDisplayOrderChanging(value);
					this.SendPropertyChanging();
					this._DisplayOrder = value;
					this.SendPropertyChanged("DisplayOrder");
					this.OnDisplayOrderChanged();
				}
			}
		}
		
		[DataMember(Order=9)]
		public System.DateTime CreatedOn
		{
			get
			{
				return this._CreatedOn;
			}
			set
			{
				if ((this._CreatedOn != value))
				{
					this.OnCreatedOnChanging(value);
					this.SendPropertyChanging();
					this._CreatedOn = value;
					this.SendPropertyChanged("CreatedOn");
					this.OnCreatedOnChanged();
				}
			}
		}
		
		[DataMember(Order=10)]
		public string CreatedBy
		{
			get
			{
				return this._CreatedBy;
			}
			set
			{
				if ((this._CreatedBy != value))
				{
					this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}
			}
		}
		
		[DataMember(Order=11)]
		public System.Nullable<System.DateTime> ModifiedOn
		{
			get
			{
				return this._ModifiedOn;
			}
			set
			{
				if ((this._ModifiedOn != value))
				{
					this.OnModifiedOnChanging(value);
					this.SendPropertyChanging();
					this._ModifiedOn = value;
					this.SendPropertyChanged("ModifiedOn");
					this.OnModifiedOnChanged();
				}
			}
		}
		
		[DataMember(Order=12)]
		public string ModifiedBy
		{
			get
			{
				return this._ModifiedBy;
			}
			set
			{
				if ((this._ModifiedBy != value))
				{
					this.OnModifiedByChanging(value);
					this.SendPropertyChanging();
					this._ModifiedBy = value;
					this.SendPropertyChanged("ModifiedBy");
					this.OnModifiedByChanged();
				}
			}
		}
		
		[DataMember(Order=13, EmitDefaultValue=false)]
		public EntitySet<Meta> Metas
		{
			get
			{
				if ((this.serializing 
							&& (this._Metas.HasLoadedOrAssignedValues == false)))
				{
					return null;
				}
				return this._Metas;
			}
			set
			{
				this._Metas.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Metas(Meta entity)
		{
			this.SendPropertyChanging();
			entity.Page = this;
		}
		
		private void detach_Metas(Meta entity)
		{
			this.SendPropertyChanging();
			entity.Page = null;
		}
		
		private void Initialize()
		{
			this._Metas = new EntitySet<Meta>(new Action<Meta>(this.attach_Metas), new Action<Meta>(this.detach_Metas));
			OnCreated();
		}
		
		[OnDeserializing()]
		[System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}
		
		[OnSerializing()]
		[System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnSerializing(StreamingContext context)
		{
			this.serializing = true;
		}
		
		[OnSerialized()]
		[System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnSerialized(StreamingContext context)
		{
			this.serializing = false;
		}
	}
	
	[DataContract()]
	public partial class Meta : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _MetaId;
		
		private System.Nullable<System.Guid> _PageId;
		
		private string _Name;
		
		private string _Content;
		
		private EntityRef<Page> _Page;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnMetaIdChanging(int value);
    partial void OnMetaIdChanged();
    partial void OnPageIdChanging(System.Nullable<System.Guid> value);
    partial void OnPageIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnContentChanging(string value);
    partial void OnContentChanged();
    #endregion
		
		public Meta()
		{
			this.Initialize();
		}
		
		[DataMember(Order=1)]
		public int MetaId
		{
			get
			{
				return this._MetaId;
			}
			set
			{
				if ((this._MetaId != value))
				{
					this.OnMetaIdChanging(value);
					this.SendPropertyChanging();
					this._MetaId = value;
					this.SendPropertyChanged("MetaId");
					this.OnMetaIdChanged();
				}
			}
		}
		
		[DataMember(Order=2)]
		public System.Nullable<System.Guid> PageId
		{
			get
			{
				return this._PageId;
			}
			set
			{
				if ((this._PageId != value))
				{
					if (this._Page.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnPageIdChanging(value);
					this.SendPropertyChanging();
					this._PageId = value;
					this.SendPropertyChanged("PageId");
					this.OnPageIdChanged();
				}
			}
		}
		
		[DataMember(Order=3)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[DataMember(Order=4)]
		public string Content
		{
			get
			{
				return this._Content;
			}
			set
			{
				if ((this._Content != value))
				{
					this.OnContentChanging(value);
					this.SendPropertyChanging();
					this._Content = value;
					this.SendPropertyChanged("Content");
					this.OnContentChanged();
				}
			}
		}
		
		public Page Page
		{
			get
			{
				return this._Page.Entity;
			}
			set
			{
				Page previousValue = this._Page.Entity;
				if (((previousValue != value) 
							|| (this._Page.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Page.Entity = null;
						previousValue.Metas.Remove(this);
					}
					this._Page.Entity = value;
					if ((value != null))
					{
						value.Metas.Add(this);
						this._PageId = value.PageId;
					}
					else
					{
						this._PageId = default(Nullable<System.Guid>);
					}
					this.SendPropertyChanged("Page");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void Initialize()
		{
			this._Page = default(EntityRef<Page>);
			OnCreated();
		}
		
		[OnDeserializing()]
		[System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}
	}
}
#pragma warning restore 1591
