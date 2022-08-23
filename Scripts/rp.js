(() => {
  $('#weeklyRecruitmentChart').kendoChart({
    dataSource: {
      error: function () {
        alert('Error');
      },
      transport: {
        read: {
          dataType: 'json',
          url: '/Report/GetWeeklyHire',
        }
      },
    },
    chartArea: {
      height: 600
    },
    legend: {
      visible: false
    },
    seriesDefaults: {
      type: 'column'
    },
    series: [{
      categoryField: 'Week',
      color: '#007bff',
      field: 'Quantity',
      name: 'Employees'
    }],
    categoryAxis: {
      labels: {
        template: 'Week #: value #',
      },
    },
    valueAxis: {
      labels: {
        rotation: 'auto',
      }
    },
    tooltip: {
      template: '#= value #',
      visible: true,
    },
  });

  $.ajax({
    dataType: 'json',
    error: function () {
      alert('Error');
    },
    success: function (response) {
      $('#lblCount').text(kendo.toString(response[0].TerminatedCount, 'n0'));
    },
    url: '/Report/GetTerminatedCurrentYear'
  });
})();
