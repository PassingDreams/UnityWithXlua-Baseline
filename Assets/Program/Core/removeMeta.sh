# 移除当前目录及其递归子目录中的所有.meta文件
find . -type f -name "*.meta" -delete

echo "所有扩展名为.meta的文件已被移除"