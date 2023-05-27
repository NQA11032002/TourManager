const ctx = document.getElementById('myChart');

new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ['Day 1', 'Day 2', 'Day 3', 'Day 4', 'Day 5', 'Day 6'],
        datasets: [{
            label: '# of Votes',
            data: [12, 19, 3, 5, 2, 3],
            borderWidth: 1
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

const pie = document.getElementById('myChart_1');

const data = {
    datasets: [{
        data: [10, 20]
    }],

    // These labels appear in the legend and in the tooltips when hovering different arcs
    labels: [
        'Tour',
        'Social',
    ]
};

const config = {
    type: 'pie',
    data: data,
};


new Chart(pie, config);
