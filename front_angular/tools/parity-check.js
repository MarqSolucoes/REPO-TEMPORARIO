const fs = require('fs');
const path = require('path');

const vuePagesDir = path.resolve(__dirname, '../../FRONT/src/pages');
const ngPagesDir = path.resolve(__dirname, '../src/app/pages');
const outFile = path.resolve(__dirname, '../MIGRATION_PARITY_REPORT.md');

function listFiles(dir, ext, base = dir) {
  let out = [];
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) out = out.concat(listFiles(full, ext, base));
    else if (entry.name.endsWith(ext)) out.push(path.relative(base, full));
  }
  return out;
}

const vuePages = listFiles(vuePagesDir, '.vue').map((f) => f.replace(/\.vue$/, '').toLowerCase());
const ngPages = listFiles(ngPagesDir, '.ts')
  .filter((f) => f.endsWith('.component.ts'))
  .map((f) => f.replace(/\.component\.ts$/, '').toLowerCase());

const matched = vuePages.filter((v) => ngPages.some((n) => n.includes(path.basename(v))));
const missing = vuePages.filter((v) => !matched.includes(v));

const lines = [];
lines.push('# Migration Parity Report');
lines.push('');
lines.push(`- Vue pages: **${vuePages.length}**`);
lines.push(`- Angular component pages: **${ngPages.length}**`);
lines.push(`- Heuristic matches: **${matched.length}**`);
lines.push(`- Missing by heuristic: **${missing.length}**`);
lines.push('');
lines.push('## Missing (heuristic)');
for (const m of missing) lines.push(`- ${m}`);

fs.writeFileSync(outFile, lines.join('\n'));
console.log('Report generated at', outFile);
