(function () {
  function prepareCanvas(canvas) {
    const ratio = window.devicePixelRatio || 1;
    const rect = canvas.getBoundingClientRect();
    canvas.width = rect.width * ratio;
    canvas.height = rect.height * ratio;
    const context = canvas.getContext("2d");
    context.scale(ratio, ratio);
    return { context, width: rect.width, height: rect.height };
  }

  function drawEmpty(context, width, height, message) {
    context.clearRect(0, 0, width, height);
    context.fillStyle = "#64748b";
    context.font = "600 14px system-ui, sans-serif";
    context.textAlign = "center";
    context.fillText(message, width / 2, height / 2);
  }

  function drawMonthlyChart() {
    const charts = window.moneyManagerCharts;
    const canvas = document.getElementById("monthlyChart");
    if (!canvas || !charts) {
      return;
    }

    const { context, width, height } = prepareCanvas(canvas);
    const data = charts.monthly || [];
    const max = Math.max(...data.flatMap((x) => [x.income, x.expenses]), 0);
    if (!max) {
      drawEmpty(context, width, height, "No data for this chart yet");
      return;
    }

    const padding = { top: 18, right: 20, bottom: 44, left: 48 };
    const plotWidth = width - padding.left - padding.right;
    const plotHeight = height - padding.top - padding.bottom;
    const groupWidth = plotWidth / data.length;
    const barWidth = Math.min(28, groupWidth / 4);

    context.clearRect(0, 0, width, height);
    context.strokeStyle = "#e5e7eb";
    context.lineWidth = 1;
    context.fillStyle = "#64748b";
    context.font = "600 12px system-ui, sans-serif";
    context.textAlign = "right";

    for (let i = 0; i <= 4; i += 1) {
      const y = padding.top + (plotHeight / 4) * i;
      const value = Math.round(max - (max / 4) * i);
      context.beginPath();
      context.moveTo(padding.left, y);
      context.lineTo(width - padding.right, y);
      context.stroke();
      context.fillText(value.toString(), padding.left - 8, y + 4);
    }

    data.forEach((item, index) => {
      const center = padding.left + groupWidth * index + groupWidth / 2;
      const incomeHeight = (item.income / max) * plotHeight;
      const expenseHeight = (item.expenses / max) * plotHeight;

      context.fillStyle = "#16a34a";
      context.fillRect(center - barWidth - 3, padding.top + plotHeight - incomeHeight, barWidth, incomeHeight);
      context.fillStyle = "#dc2626";
      context.fillRect(center + 3, padding.top + plotHeight - expenseHeight, barWidth, expenseHeight);

      context.fillStyle = "#64748b";
      context.textAlign = "center";
      context.fillText(item.label, center, height - 18);
    });
  }

  function drawCategoryChart() {
    const charts = window.moneyManagerCharts;
    const canvas = document.getElementById("categoryChart");
    if (!canvas || !charts) {
      return;
    }

    const { context, width, height } = prepareCanvas(canvas);
    const data = charts.categories || [];
    const total = data.reduce((sum, item) => sum + item.amount, 0);
    if (!total) {
      drawEmpty(context, width, height, "No spending data yet");
      return;
    }

    const colors = ["#2563eb", "#16a34a", "#f59e0b", "#dc2626", "#7c3aed", "#0891b2"];
    const radius = Math.min(width, height) * 0.28;
    const cx = width * 0.34;
    const cy = height / 2;
    let start = -Math.PI / 2;

    context.clearRect(0, 0, width, height);

    data.forEach((item, index) => {
      const slice = (item.amount / total) * Math.PI * 2;
      context.beginPath();
      context.moveTo(cx, cy);
      context.arc(cx, cy, radius, start, start + slice);
      context.closePath();
      context.fillStyle = colors[index % colors.length];
      context.fill();
      start += slice;
    });

    context.beginPath();
    context.arc(cx, cy, radius * 0.58, 0, Math.PI * 2);
    context.fillStyle = "#ffffff";
    context.fill();

    context.font = "700 12px system-ui, sans-serif";
    context.textAlign = "left";
    data.forEach((item, index) => {
      const y = 44 + index * 30;
      context.fillStyle = colors[index % colors.length];
      context.fillRect(width * 0.64, y - 10, 12, 12);
      context.fillStyle = "#172033";
      context.fillText(item.label, width * 0.64 + 20, y);
    });
  }

  function renderCharts() {
    drawMonthlyChart();
    drawCategoryChart();
  }

  window.addEventListener("resize", renderCharts);
  window.addEventListener("DOMContentLoaded", renderCharts);
})();
