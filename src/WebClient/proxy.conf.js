module.exports = {
  "/api": {
    target:
      process.env["services__BaustellenAppBackendApi__https__0"] ||
      process.env["services__BaustellenAppBackendApi__http__0"],
    secure: process.env["NODE_ENV"] !== "development",
    pathRewrite: {
      "^/api": "",
    },
  },
};
