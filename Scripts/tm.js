(() => {
  const MAX_FILE_SIZE = 10485760;            // 10 MB

  let departments, employmentStatus, positions, shifts, managers;

  $.ajax({
    dataType: 'json',
    error: function () {
      departments = [];
      alert('Error');
    },
    success: function (response) {
      departments = response;

      $('#filterDepartment').kendoDropDownList({
        change: function (e) {
          if (e.sender.value() !== 'No filter') {
            $('#filterManager').getKendoDropDownList().select(0);

            gridTeamMember.dataSource.filter({
              field: 'DepartmentName',
              operator: 'eq',
              value: e.sender.value()
            });
          } else {
            gridTeamMember.dataSource.filter({});
          }
        },
        dataSource: departments.map(d => d.Name),
        optionLabel: 'No filter'
      });
    },
    url: '/Department/Get'
  });

  $.ajax({
    dataType: 'json',
    error: function () {
      employmentStatus = [];
      alert('Error');
    },
    success: function (response) {
      employmentStatus = response;
    },
    url: '/Home/GetEmploymentStatus'
  });

  $.ajax({
    dataType: 'json',
    error: function () {
      positions = [];
      alert('Error');
    },
    success: function (response) {
      positions = response;
    },
    url: '/Position/Get'
  });

  $.ajax({
    dataType: 'json',
    error: function () {
      shifts = [];
      alert('Error');
    },
    success: function (response) {
      shifts = response;
    },
    url: '/Home/GetShift'
  });

  const gridTeamMember = $('#gridTeamMember').kendoGrid({
    columns: [
      {
        attributes: {
          'class': 'text-center',
        },
        command: [{
          template: '<button type="button" class="btn btn-sm btn-danger k-grid-delete"><i class="fa fa-remove" aria-hidden="true"></i></button>',
        }],
        title: 'Delete',
        width: 70
      },
      { field: 'Name', width: 150 },
      {
        field: 'Address',
        filterable: false,
        sortable: false,
        width: 200
      },
      {
        field: 'EmailAddress',
        filterable: false,
        sortable: false,
        title: 'Email Address',
        width: 150
      },
      {
        field: 'PhoneNumber',
        filterable: false,
        sortable: false,
        title: 'Phone Number',
        width: 150
      },
      {
        field: 'Position',
        filterable: false,
        editor: function (container, options) {
          const data = gridTeamMember.dataItem($(container).closest('tr'));

          $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
              dataSource: {
                data: positions
              },
              dataTextField: 'Name',
              dataValueField: 'Id',
              value: data.Position ? data.Position.Id : null,
            });
        },
        template: '#: Position ? Position.Name : "" #',
        width: 160
      },
      {
        field: 'Department',
        filterable: false,
        editor: function (container, options) {
          const data = gridTeamMember.dataItem($(container).closest('tr'));

          $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
              dataSource: {
                data: departments
              },
              dataTextField: 'Name',
              dataValueField: 'Id',
              value: data.Department ? data.Department.Id : null,
            });
        },
        template: '#: Department ? Department.Name : "" #',
        width: 170
      },
      {
        field: 'StartDate',
        filterable: false,
        format: '{0:MMM d, yyyy}',
        sortable: false,
        title: 'Start Date',
        type: 'date',
        width: 110
      },
      {
        field: 'EndDate',
        filterable: false,
        format: '{0:MMM d, yyyy}',
        sortable: false,
        title: 'End Date',
        type: 'date',
        width: 110
      },
      {
        field: 'EmploymentStatus',
        filterable: false,
        editor: function (container, options) {
          const data = gridTeamMember.dataItem($(container).closest('tr'));

          $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
              dataSource: {
                data: employmentStatus
              },
              dataTextField: 'Name',
              dataValueField: 'Id',
              value: data.EmploymentStatus ? data.EmploymentStatus.Id : null,
            });
        },
        template: '#: EmploymentStatus ? EmploymentStatus.Name : "" #',
        title: 'Employment Status',
        width: 200
      },
      {
        field: 'Shift',
        filterable: false,
        editor: function (container, options) {
          const data = gridTeamMember.dataItem($(container).closest('tr'));

          $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
              dataSource: {
                data: shifts
              },
              dataTextField: 'Name',
              dataValueField: 'Id',
              value: data.Shift ? data.Shift.Id : null,
            });
        },
        template: '#: Shift ? Shift.Name : "" #',
        width: 120
      },
      {
        field: 'Manager',
        filterable: false,
        editor: function (container, options) {
          const data = gridTeamMember.dataItem($(container).closest('tr'));
          const tempManagers = managers;
          const index = tempManagers.findIndex(d => d.Id === data.Id);

          tempManagers.splice(index, 1);

          $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
              dataSource: {
                data: tempManagers
              },
              dataTextField: 'Name',
              dataValueField: 'Id',
              optionLabel: {
                Id: '',
                Name: '',
              },
              value: data.Manager ? data.Manager.Id : null,
            });
        },
        template: '#: Manager ? Manager.Name : "" #',
        width: 180
      },
      {
        field: 'Photo',
        filterable: false,
        sortable: false,
        template: kendo.template($('#memberPhotoTemplate').html()),
        width: 150
      },
      {
        field: 'FavoriteColor',
        filterable: false,
        sortable: false,
        title: 'Favorite Color',
        width: 170
      },
    ],
    dataBound: function (e) {
      if (e.sender.dataSource.total() > 0) {
        managers = e.sender.dataSource.data().toJSON().map(d => {
          return {
            Id: d.Id,
            Name: d.Name,
          };
        });
        managers = e.sender.dataSource.data().toJSON().reduce((prev, current) => {
          if (current.Manager) {
            if (!prev.some(p => p.Id === current.Manager.Id)) {
              prev.push({
                Id: current.Manager.Id,
                Name: current.Manager.Name,
              });
            }
          }

          return prev;
        }, []);
      } else {
        managers = [];
      }

      $('#filterManager').kendoDropDownList({
        change: function (e) {
          if (e.sender.value() !== 'No filter') {
            $('#filterDepartment').getKendoDropDownList().select(0);

            gridTeamMember.dataSource.filter({
              field: 'ManagerName',
              operator: 'eq',
              value: e.sender.value()
            });
          } else {
            gridTeamMember.dataSource.filter({});
          }
        },
        dataSource: managers.map(m => m.Name),
        optionLabel: 'No filter'
      });
    },
    dataSource: {
      change: function (e) {
        if (e.action === 'remove') {
          e.sender.sync();
        }
      },
      error: function () {
        alert('Error');
      },
      pageSize: 10,
      schema: {
        model: {
          id: 'Id',
          fields: {
            Id: { editable: false, nullable: false },
            Name: { type: 'string', validation: { required: true } },
            Address: { nullable: true, type: 'string' },
            EmailAddress: { nullable: true, type: 'string' },
            PhoneNumber: { nullable: true, type: 'string' },
            Position: { nullable: true },
            Department: { nullable: true },
            StartDate: { nullable: true, type: 'date' },
            EndDate: { editable: false, nullable: true, type: 'date' },
            EmploymentStatus: { nullable: true },
            Shift: { nullable: true },
            Manager: { nullable: true },
            Photo: { editable: false, nullable: true, type: 'string' },
            FavoriteColor: { nullable: true, type: 'string' },
          }
        },
        total: function (d) {
          return d.length;
        },
      },
      sort: {
        field: 'Name',
        dir: 'asc',
      },
      transport: {
        parameterMap: function (d) {
          return JSON.stringify(d);
        },
        create: {
          contentType: 'application/json',
          dataType: 'json',
          type: 'POST',
          url: '/Home/AddTeamMember'
        },
        destroy: {
          contentType: 'application/json',
          dataType: 'json',
          type: 'POST',
          url: '/Home/DeleteTeamMember'
        },
        read: {
          contentType: 'application/json',
          dataType: 'json',
          url: '/Home/GetAllTeamMember'
        },
        update: {
          contentType: 'application/json',
          dataType: 'json',
          type: 'POST',
          url: '/Home/UpdateTeamMember'
        }
      }
    },
    editable: true,
    filterable: true,
    navigatable: true,
    pageable: {
      pageSize: 10,
      refresh: true,
    },
    resizable: true,
    selectable: true,
    sortable: true,
    toolbar: kendo.template($('#memberGridToolbarTemplate').html())
  }).getKendoGrid();

  const dialogActivity = $('#dialogActivity').kendoWindow({
    modal: true,
    open: function (e) {
      const dlg = e.sender.wrapper;

      dlg.css('left', ($(window).width() - dlg.width()) / 2);
      dlg.css('top', window.pageYOffset + 50);
    },
    resizable: false,
    title: 'Activity Log',
    visible: false,
    width: 1000,
  }).getKendoWindow();

  const gridActivityLog = $('#gridActivityLog').kendoGrid({
    autoBind: false,
    columns: [
      { field: 'Type' },
      { field: 'OldValue', title: 'Old Value' },
      { field: 'NewValue', title: 'New Value' },
      { field: 'ModifiedDate', format: '{0:MMM d, yyyy}', title: 'Modified Date', type: 'date' },
    ],
    dataSource: {
      error: function () {
        alert('Error');
      },
      pageSize: 20,
      schema: {
        total: function (d) {
          return d.length;
        },
      },
      transport: {
        parameterMap: function (d) {
          return JSON.stringify(d);
        },
        read: {
          contentType: 'application/json',
          data: function () {
            return {
              MemberId: gridTeamMember.dataItem(gridTeamMember.select()[0]).Id
            };
          },
          dataType: 'json',
          type: 'POST',
          url: '/Home/GetActivityLog'
        }
      }
    },
    pageable: {
      pageSize: 20,
    },
  }).getKendoGrid();

  const dialogPhoto = $('#dialogPhoto').kendoWindow({
    modal: true,
    open: function (e) {
      const dlg = e.sender.wrapper;

      dlg.css('left', ($(window).width() - dlg.width()) / 2);
      dlg.css('top', window.pageYOffset + 50);
    },
    resizable: false,
    title: 'Photo Upload',
    visible: false,
    width: 600,
  }).getKendoWindow();

  const photo = $('#photo').kendoUpload({
    async: {
      autoUpload: true,
      saveUrl: '/Home/UploadFile'
    },
    error: function () {
      alert('Error');
    },
    select: function (e) {
      for (let i = 0; i < e.files.length; i++) {
        const ext = e.files[i].extension.toLowerCase();

        if (ext !== '.png' && ext !== '.gif' && ext !== '.jpg') {
          e.preventDefault();
          alert('File type not supported');
        }
        else if (e.files[i].size > MAX_FILE_SIZE) {
          e.preventDefault();
          alert('File too large');
        }
      }
    },
    success: function (e) {
      if (e.response.FilePath) {
        gridTeamMember.dataSource.get(gridTeamMember.dataItem(gridTeamMember.select()[0]).Id).Photo = e.response.FilePath;
        gridTeamMember.refresh();
      }

      dialogPhoto.close();
    },
    upload: function (e) {
      e.data = {
        MemberId: gridTeamMember.dataItem(gridTeamMember.select()[0]).Id
      };
    }
  }).getKendoUpload();

  const dialogPermission = $('#dialogPermission').kendoWindow({
    modal: true,
    open: function (e) {
      const dlg = e.sender.wrapper;

      dlg.css('left', ($(window).width() - dlg.width()) / 2);
      dlg.css('top', window.pageYOffset + 50);
    },
    resizable: false,
    title: 'Set Member Permission',
    visible: false,
    width: 600,
  }).getKendoWindow();

  const permission = $('#permission').kendoMultiSelect({
    dataSource: {
      error: function () {
        alert('Error');
      },
      transport: {
        read: {
          contentType: 'application/json',
          dataType: 'json',
          url: '/Home/GetPermission'
        },
      }
    },
    dataTextField: 'Name',
    dataValueField: 'Id',
    placeholder: 'Select...'
  }).getKendoMultiSelect();

  $('.k-grid-terminate').click(function () {
    if (gridTeamMember.select().length > 0) {
      if (confirm('Terminate selected member?')) {
        $.ajax({
          contentType: 'application/json',
          data: JSON.stringify({ MemberId: gridTeamMember.dataItem(gridTeamMember.select()[0]).Id }),
          dataType: 'json',
          error: function () {
            alert('Error');
          },
          success: function () {
            gridTeamMember.dataSource.read();
          },
          type: 'POST',
          url: '/Home/TerminateTeamMember'
        });
      }
    } else {
      alert('Select team member to terminate.');
    }
  });

  $('.k-grid-set-permission').click(function () {
    if (gridTeamMember.select().length > 0) {
      $.ajax({
        contentType: 'application/json',
        data: JSON.stringify({ MemberId: gridTeamMember.dataItem(gridTeamMember.select()[0]).Id }),
        dataType: 'json',
        error: function () {
          alert('Error');
        },
        success: function (e) {
          permission.value(e.map(p => p.PermissionId));
          dialogPermission.open();
        },
        type: 'POST',
        url: '/Home/GetMemberPermissions'
      });
    } else {
      alert('Select team member first.');
    }
  });

  $('.k-grid-activity-log').click(function () {
    if (gridTeamMember.select().length > 0) {
      gridActivityLog.dataSource.read();
      dialogActivity.open();
    } else {
      alert('Select team member to view log.');
    }
  });

  $('body').on('click', '.upload-photo', function (e) {
    e.preventDefault();
    gridTeamMember.select($(e.target).closest('tr'));
    photo.clearAllFiles();
    dialogPhoto.open();
  });

  $('#closePermission').click(function () {
    dialogPermission.close();
  });

  $('#savePermission').click(function () {
    $.ajax({
      contentType: 'application/json',
      data: JSON.stringify({
        MemberId: gridTeamMember.dataItem(gridTeamMember.select()[0]).Id,
        PermissionIds: permission.value()
      }),
      dataType: 'json',
      error: function () {
        alert('Error');
      },
      success: function () {
        dialogPermission.close();
      },
      type: 'POST',
      url: '/Home/SetMemberPermission'
    });
  });
})();
