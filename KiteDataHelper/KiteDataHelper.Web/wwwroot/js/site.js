// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";
var instruments;
var connection;
var selectedInstrument;
var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
var todaysDate = new Date();
var month = months[todaysDate.getMonth()];
var date = todaysDate.getDate().toString();
var year = todaysDate.getFullYear().toString();
var countDownDateString = month + ' ' + date + ', ' + year + ' 15:45:00';
var countDownDate = new Date(countDownDateString).getTime();

function closeWebSocket() {
    connection.invoke("CloseTicks").catch(function (err) {
        return console.error(err.toString());
    });
}

$(document).ready(async function () {
    var rowTemplate = '<tr id="{orderId}"><td>{orderId}</td>\
                        <td>{executionTime}</td><td>{positionType}</td><td>{tradeType}</td><td style="word-wrap:break-word;">{tradeReason}</td><td>{tradingSymbol}</td><td>{strikePrice}</td><td>{averagePrice}</td>\
                        <td>{lastPrice}</td><td>{plusDmiValueAtPosition}</td><td>{minusDmiValueAtPosition}</td><td>{plusDmiIndexAtPosition}</td><td>{minusDmiIndexAtPosition}</td><td>{plusDmiSlope}</td><td>{minusDmiSlope}</td><td>{profitLoss}</td></tr>';
    var colTemplate = '<td>{orderId}</td>\
                        <td>{executionTime}</td><td>{positionType}</td><td>{tradeType}</td><td style="word-wrap:break-word;">{tradeReason}</td><td>{tradingSymbol}</td><td>{strikePrice}</td><td>{averagePrice}</td>\
                        <td>{lastPrice}</td><td>{plusDmiValueAtPosition}</td><td>{minusDmiValueAtPosition}</td><td>{plusDmiIndexAtPosition}</td><td>{minusDmiIndexAtPosition}</td><td>{plusDmiSlope}</td><td>{minusDmiSlope}</td><td>{profitLoss}</td>';
    if (!String.prototype.supplant) {
        String.prototype.supplant = function (o) {
            return this.replace(/{([^{}]*)}/g,
                function (a, b) {
                    var r = o[b];
                    return typeof r === 'string' || typeof r === 'number' ? r : a;
                }
            );
        };
    }

    if (_isKiteConnected.value === 'True') {
        connection = new signalR.HubConnectionBuilder().withUrl("/tickers").build();
        //const chart = LightweightCharts.createChart(document.getElementById('chart'), { width: 750, height: 800 });
        //const candleStickSeries = chart.addCandlestickSeries();
        async function start() {
            try {
                await connection.start();
                console.log("SignalR Connected");
                //startTicks();
                document.getElementById("marketStatus").src = '/images/statusgreen.png';
            } catch (err) {
                console.log(err);
                setTimeout(start, 5000);
            }
        };

        connection.onclose(start);

        connection.on("ReceiveMessage", function (message) {
            console.log(message);
            candleStickSeries.update(message);
        });

        connection.on("PositionUpdate", function (message) {

            message.time = new Date(message.time).toLocaleTimeString();
            if ($('#' + String(message.orderId)).length > 0) {
                $('#' + String(message.orderId) + ' td').remove();
                $('#' + String(message.orderId)).append(colTemplate.supplant(message));
            }
            else {
                var value = rowTemplate.supplant(message);
                $('#positiondetails > tbody').append(value);
            }

            if (parseFloat(message.profitLoss) > 0) {
                $('#' + String(message.orderId) + 'td:nth-child(15)').attr('style', 'color:green;');
            }
            else {
                $('#' + String(message.orderId) + 'td:nth-child(15)').attr('style', 'color:red;');
            }
        });

        function startServerTimer() {
            var x = setInterval(function () {

                // Get today's date and time
                var now = new Date().getTime();

                // Find the distance between now and the count down date
                var distance = countDownDate - now;

                // Time calculations for days, hours, minutes and seconds
                var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);
                $('#timeLeft').text(minutes + ':' + seconds);

                // If the count down is finished, write some text
                if (distance < 0) {
                    clearInterval(x);
                    $('#timeLeft').text('');
                    $.ajax("/home/StartDataTimers").done(function (result) {
                        console.log(result);
                    }).fail(function (message) {
                        console.log(message);
                    });
                }
            }, 1000);
        }

        async function startTicks() {
            try {
                await connection.invoke("StartTicks", '260105');
            } catch (err) {
                console.error(err);
            }
        }
        
        start();
        startServerTimer();
    }
});
