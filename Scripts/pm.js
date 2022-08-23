(() => {
  $('#gridMemberPermission').kendoGrid({
    columns: [
      { field: 'MemberName', width: 300 },
      { field: 'Permission', filterable: false },
    ],
    dataSource: {
      error: function () {
        alert('Error');
      },
      pageSize: 100,
      schema: {
        total: function (d) {
          return d.length;
        },
      },
      transport: {
        read: {
          contentType: 'application/json',
          dataType: 'json',
          url: '/Permission/GetMemberPermissions'
        },
      }
    },
    filterable: true,
    pageable: {
      pageSize: 100,
      refresh: true,
    },
    resizable: true,
    selectable: true,
  });
})();
