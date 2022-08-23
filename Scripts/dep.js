(() => {
  const gridDepartment = $('#gridDepartment').kendoGrid({
    columns: [
      //{
      //  attributes: {
      //    'class': 'text-center',
      //  },
      //  command: [{
      //    template: '<button type="button" class="btn btn-sm btn-danger k-grid-delete"><i class="fa fa-remove" aria-hidden="true"></i></button>',
      //  }],
      //  title: 'Delete',
      //  width: 70
      //},
      { field: 'Name' },
    ],
    dataSource: {
      change: function (e) {
        if (e.action === 'itemchange') {
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
          url: '/Department/Add'
        },
        //destroy: {
        //  contentType: 'application/json',
        //  dataType: 'json',
        //  type: 'POST',
        //  url: '/Department/Delete'
        //},
        read: {
          contentType: 'application/json',
          dataType: 'json',
          url: '/Department/Get'
        },
        update: {
          contentType: 'application/json',
          dataType: 'json',
          type: 'POST',
          url: '/Department/Update'
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
    toolbar: kendo.template($('#departmentGridToolbarTemplate').html())
  }).getKendoGrid();
})();
